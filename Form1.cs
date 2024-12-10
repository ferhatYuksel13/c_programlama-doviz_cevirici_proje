using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Net;

namespace DovizCevirici
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=LEGEND;Initial Catalog=DovizDB;Integrated Security=True;Encrypt=False;";
        private string logFilePath = "error_log.txt"; // Hata kaydının yazılacağı dosya yolu

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadCurrencyCodes(); // Döviz kodlarını yükle
            lblResult.Text = "Döviz Çeviriciye Hoşgeldiniz!";
            lblLastUpdated.Text = "Son güncelleme: Henüz yapılmadı";
            await UpdateExchangeRates(); // Döviz oranlarını güncelle
        }

        private async void btnUpdateRates_Click(object sender, EventArgs e)
        {
            await UpdateExchangeRates(); // Döviz oranlarını güncelle
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ConvertCurrency(); // Döviz çevirme işlemini yap
        }

        // Döviz kodlarını veritabanından yükle ve ComboBox'ları doldur
        private void LoadCurrencyCodes()
        {
            try
            {
                cmbFromCurrency.Items.Clear();
                cmbToCurrency.Items.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT currency_code FROM exchange_rates"; // exchange_rates tablosundaki döviz kodlarını al
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string currencyCode = reader["currency_code"].ToString();
                            cmbFromCurrency.Items.Add(currencyCode);
                            cmbToCurrency.Items.Add(currencyCode);
                        }
                    }
                }

                if (cmbFromCurrency.Items.Count > 0)
                {
                    cmbFromCurrency.SelectedIndex = 0;
                    cmbToCurrency.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Döviz oranlarını TCMB API'sinden al ve veritabanını güncelle
        private async Task UpdateExchangeRates()
        {
            try
            {
                // Loading spinner'ı göster
                ShowLoadingSpinner(true);

                string url = "https://www.tcmb.gov.tr/kurlar/today.xml"; // TCMB döviz verisi XML URL'si
                HttpClient client = new HttpClient();
                string xmlData = await client.GetStringAsync(url); // XML verisini indir
                XDocument xdoc = XDocument.Parse(xmlData); // XML verisini parse et

                var currencies = xdoc.Descendants("Currency")
                                     .Where(c => c.Element("ForexBuying") != null) // ForexBuying elementini kontrol et
                                     .Select(c => new
                                     {
                                         Code = c.Attribute("CurrencyCode").Value, // Döviz kodu
                                         Rate = decimal.Parse(c.Element("ForexBuying").Value) // Döviz kuru
                                     }).ToList();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var currency in currencies)
                    {
                        string query = "SELECT rate FROM exchange_rates WHERE currency_code = @currency_code"; // Veritabanında döviz kodu araması
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@currency_code", currency.Code);
                            var currentRate = cmd.ExecuteScalar();

                            // Eğer döviz oranı değişmişse, güncelleme yap
                            if (currentRate == null)
                            {
                                // Döviz kodu yoksa yeni ekle
                                string insertQuery = "INSERT INTO exchange_rates (currency_code, rate, updated_at) VALUES (@currency_code, @rate, GETDATE())";
                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                                {
                                    insertCmd.Parameters.AddWithValue("@currency_code", currency.Code);
                                    insertCmd.Parameters.AddWithValue("@rate", currency.Rate);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                            else if ((decimal)currentRate != currency.Rate)
                            {
                                string updateQuery = "UPDATE exchange_rates SET rate = @rate, updated_at = GETDATE() WHERE currency_code = @currency_code";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                                {
                                    updateCmd.Parameters.AddWithValue("@rate", currency.Rate);
                                    updateCmd.Parameters.AddWithValue("@currency_code", currency.Code);
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                lblLastUpdated.Text = $"Son güncelleme: {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";
                MessageBox.Show("Döviz oranları başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException webEx)
            {
                LogError(webEx.Message);
                MessageBox.Show($"API hatası: {webEx.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Loading spinner'ı gizle
                ShowLoadingSpinner(false);
            }
        }

        // Döviz çevirme işlemini yap
        private void ConvertCurrency()
        {
            if (string.IsNullOrEmpty(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir tutar girin.", "Geçersiz Tutar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fromCurrency = cmbFromCurrency.SelectedItem.ToString();
            string toCurrency = cmbToCurrency.SelectedItem.ToString();

            decimal fromRate, toRate;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT rate FROM exchange_rates WHERE currency_code = @currency_code";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@currency_code", fromCurrency);
                        fromRate = (decimal)cmd.ExecuteScalar();

                        cmd.Parameters["@currency_code"].Value = toCurrency;
                        toRate = (decimal)cmd.ExecuteScalar();
                    }
                }

                decimal convertedAmount = (amount / fromRate) * toRate;

                // Döviz çevirisini düzgün formatta göster
                lblResult.Text = $"{amount} {fromCurrency} = {convertedAmount.ToString("#,0.00")} {toCurrency}";
                lblResult.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                MessageBox.Show($"Çeviri hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Loading spinner'ı göster/gizle
        private void ShowLoadingSpinner(bool show)
        {
            // Loading spinner yok, bu metod boş bırakılabilir
        }

        // Hata loglama
        private void LogError(string errorMessage)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata kaydedilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
