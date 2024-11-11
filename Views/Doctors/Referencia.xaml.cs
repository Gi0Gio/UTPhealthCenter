using System.Collections.ObjectModel;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace HealthCare.Views.Doctors
{
    public partial class Referencia : ContentPage
    {
        public ObservableCollection<ReferenciaModel> Referencias { get; set; } = new ObservableCollection<ReferenciaModel>();

        public Command AgregarReferenciaCommand { get; }
        public Command<ReferenciaModel> EliminarReferenciaCommand { get; }
        public Command GenerarPdfCommand { get; }

        public Referencia()
        {
            InitializeComponent();

            EliminarReferenciaCommand = new Command<ReferenciaModel>(EliminarReferencia);

            BindingContext = this;
        }

        private async void AgregarReferencia(object sender, EventArgs e)
        {
            string doctorTexto;
            string especialidadTexto;
            string clinicaTexto;

            // Verificar si es Tablet o Desktop y capturar el valor adecuado
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                doctorTexto = doctor.Text;
                especialidadTexto = especialidad.Text;
                clinicaTexto = clinica.Text;
            }
            else // En caso de Desktop
            {
                doctorTexto = DoctorEntry.Text;
                especialidadTexto = EspecialidadEntry.Text;
                clinicaTexto = ClinicaEntry.Text;
            }

            // Agregar la referencia con los datos obtenidos
            var referencia = new ReferenciaModel
            {
                Doctor = doctorTexto,
                Especialidad = especialidadTexto,
                Clinica = clinicaTexto
            };

            Referencias.Add(referencia);

            // Limpiar campos
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                doctor.Text = string.Empty;
                especialidad.Text = string.Empty;
                clinica.Text = string.Empty;
            }
            else
            {
                DoctorEntry.Text = string.Empty;
                EspecialidadEntry.Text = string.Empty;
                ClinicaEntry.Text = string.Empty;
            }

            await DisplayAlert("Éxito", "La referencia ha sido añadida exitosamente.", "OK");
        }


        private void EliminarReferencia(ReferenciaModel referencia)
        {
            Referencias.Remove(referencia);
        }

        private async void GenerarPdf(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "ReferenciasClinica.pdf");
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                string logoClinicaPath = "C:\\Users\\Kris\\source\\repos\\GithubUTPhealthCare\\UTPhealthCenter\\Resources\\Images\\logoclinica.png";
                string logoUniversidadPath = "C:\\Users\\Kris\\source\\repos\\GithubUTPhealthCare\\UTPhealthCenter\\Resources\\Images\\logouniver.png";

                iText.Layout.Element.Image logoClinica = new iText.Layout.Element.Image(ImageDataFactory.Create(logoClinicaPath)).SetWidth(100);
                iText.Layout.Element.Image logoUniversidad = new iText.Layout.Element.Image(ImageDataFactory.Create(logoUniversidadPath)).SetWidth(100);

                Table headerTable = new Table(new float[] { 1, 2, 1 }).UseAllAvailableWidth();

                headerTable.AddCell(new iText.Layout.Element.Cell().Add(logoClinica)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));

                Paragraph encabezado = new Paragraph("Universidad Tecnológica de Panamá\nClínica HealthCare")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(18)
                    .SetBold();
                headerTable.AddCell(new iText.Layout.Element.Cell().Add(encabezado)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                headerTable.AddCell(new iText.Layout.Element.Cell().Add(logoUniversidad)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                document.Add(headerTable);

                document.Add(new Paragraph("\n\n\n"));


                document.Add(new Paragraph("Referencias Clínicas")
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));


                foreach (var referencia in Referencias)
                {
                    document.Add(new Paragraph()
                    .Add(new Text("Doctor: ").SetBold())
                    .Add(new Text(referencia.Doctor))
                    .Add(new Text("\n"))
                    .Add(new Text("Especialidad: ").SetBold())
                    .Add(new Text(referencia.Especialidad))
                    .Add(new Text("\n"))
                    .Add(new Text("Clínica en la que atiende: ").SetBold())
                    .Add(new Text(referencia.Clinica))
                    .SetFontSize(12)
                    .SetMarginBottom(10));
                }

                document.Close();
            }

            Referencias.Clear();


            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }

    public class ReferenciaModel
    {
        public string Doctor { get; set; }
        public string Especialidad { get; set; }
        public string Clinica { get; set; }
    }
}

