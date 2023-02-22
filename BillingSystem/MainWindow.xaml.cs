using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using BillingSystem;

namespace BillingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> /// 

    public class Invoice
    {
        public string Invoice_Id { get; set; }
        public string Product_Name { get; set; }
        public string Quantity { get; set; }
        public string Customer_Name { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public string Total_Price { get; set; }
    }

   

    public partial class MainWindow : Window
    {
        List<Invoice> invoices = new List<Invoice>();   
        public MainWindow()
        {
            InitializeComponent();

        }
        static string[] Scopes = { DocsService.Scope.Documents };
        static string ApplicationName = "Google Docs API .NET BillingSystem";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();

            invoice.Invoice_Id = Invoice_Id.Text;
            invoice.Product_Name = Product_Name.Text;
            invoice.Quantity = Quantity.Text;
            invoice.Customer_Name = Customer_Name.Text;
            invoice.Invoice_Date = Invoice_Date.Text;
            invoice.Due_Date = Due_Date.Text;
            invoice.Total_Price = Total_Price.Text;

            invoices.Add(invoice);
            DGrid.ItemsSource = null;
            LoadGrid();
        }

        private void LoadGrid()
        {
             DGrid.ItemsSource = invoices;
        }

        

       
        {
            Console.WriteLine("here");
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("credentials.json", FileMode.Open, FileAccess.ReadWrite))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "admin",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Docs API service.
                var service = new DocsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define request parameters.
                String documentId = "1MuGyT_sehufhA5EL4IN6d4jTXorsAszyGpNnM1Pr7bc";
                DocumentsResource.GetRequest request = service.Documents.Get(documentId);

                // Prints the title of the requested doc:
                //https://docs.google.com/document/d/1MuGyT_sehufhA5EL4IN6d4jTXorsAszyGpNnM1Pr7bc/edit
                Document doci = request.Execute();
                Console.WriteLine("The title of the doc is: {0}", doci.Title);
                var docId = "1MuGyT_sehufhA5EL4IN6d4jTXorsAszyGpNnM1Pr7bc";
               

                List<Request> requests = new List<Request>
                {
                    new Request()
                    {
                        InsertTable = new InsertTableRequest()
                        {
                            EndOfSegmentLocation = new EndOfSegmentLocation
                            {
                                SegmentId = ""
                            },
                            Columns = 7,
                            Rows = invoices.Count+1
                        }
                    }
                };

                BatchUpdateDocumentRequest body = new BatchUpdateDocumentRequest { Requests = requests };

                service.Documents.BatchUpdate(body, docId).Execute();

                var doc = service.Documents.Get(docId).Execute();

                var index = doc.Body.Content.FirstOrDefault(x => x.Table != null).StartIndex + 3;

                requests = new List<Request>();
                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Invoice Id",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Product Name",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Quantity",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Customer Name",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Invoice Date",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Due Date",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });
                index += 2;

                requests.Add(new Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = "Total Price",
                        Location = new Location()
                        {
                            Index = index
                        }
                    }
                });

                index += 3;

                foreach (var row in Invoice)
                {

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Invoice_Id,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Product_Name,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Quantity,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Customer_Name,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Invoice_Date,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Due_Date,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });
                    index += 2;

                    requests.Add(new Request()
                    {
                        InsertText = new InsertTextRequest()
                        {
                            Text = row.Total_Price,
                            Location = new Location()
                            {
                                Index = index
                            }
                        }
                    });

                    index += 3;
                }
                requests.Reverse();
                //var json = JsonConvert.SerializeObject(requests);
                body = new BatchUpdateDocumentRequest { Requests = requests };
                service.Documents.BatchUpdate(body, docId).Execute();
                Process.Start("https://docs.google.com/document/d/1MuGyT_sehufhA5EL4IN6d4jTXorsAszyGpNnM1Pr7bc/edit");

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
