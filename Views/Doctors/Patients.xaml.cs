using System.Collections.ObjectModel;
using System.Net.Http.Json;
using HealthCare.Models;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace HealthCare.Views.Doctors;

public partial class Patients : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients";

    public ObservableCollection<PatientDto> PatientModel { get; set; } = new ObservableCollection<PatientDto>();
    public PatientDto SelectedPatient { get; set; }

    public Patients()
    {
        InitializeComponent();
        BindingContext = this;
        LoadPatientsAsync();
    }

    private async void LoadPatientsAsync()
    {
        try
        {
            var patients = await _httpClient.GetFromJsonAsync<List<PatientDto>>(ApiUrl);
            if (patients != null)
            {
                foreach (var patient in patients)
                {
                    PatientModel.Add(patient);
                }
            }
            else
            {
                await DisplayAlert("Error", "No se encontraron pacientes.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al cargar los pacientes: {ex.Message}", "OK");
        }
    }

    private async void Pdf(object sender, EventArgs e)
    {
        if (SelectedPatient == null)
        {
            await DisplayAlert("Error", "Seleccione un paciente para generar el certificado.", "OK");
            return;
        }

        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", $"Certificado_{SelectedPatient.FullName}.pdf");

        using (PdfWriter writer = new PdfWriter(filePath))
        {
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Paths to logos
            string logoClinicaPath = "C:\\Users\\manue\\Desktop\\ClinicaApp\\UTPhealthCenter\\Resources\\Images\\logoclinica.png";
            string logoUniversidadPath = "C:\\Users\\manue\\Desktop\\ClinicaApp\\UTPhealthCenter\\Resources\\Images\\logouniver.png";

            // Add logos
            var logoClinica = new iText.Layout.Element.Image(ImageDataFactory.Create(logoClinicaPath)).SetWidth(100);
            var logoUniversidad = new iText.Layout.Element.Image(ImageDataFactory.Create(logoUniversidadPath)).SetWidth(100);

            // Header Table
            var headerTable = new Table(new float[] { 1, 2, 1 }).UseAllAvailableWidth();
            headerTable.AddCell(new iText.Layout.Element.Cell().Add(logoClinica).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            headerTable.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph("Universidad Tecnológica de Panamá\nClínica HealthCare")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(18).SetBold())
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            headerTable.AddCell(new iText.Layout.Element.Cell().Add(logoUniversidad).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            document.Add(headerTable);
            document.Add(new Paragraph("\n\n"));

            // Add date and patient details
            var fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            var contenido = $"Certifico que el/la paciente {SelectedPatient.FullName}, con cédula {SelectedPatient.Dni}, " +
                            $"residente en {SelectedPatient.Address}, ha sido evaluado/a en esta clínica " +
                            "y se encuentra en buen estado de salud.\n\n" +
                            "Se expide el presente certificado a solicitud del interesado.";

            document.Add(new Paragraph($"Fecha: {fechaActual}\n\n"));
            document.Add(new Paragraph(contenido).SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED));

            // Add signatures
            var firmaTable = new Table(2).UseAllAvailableWidth();
            firmaTable.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph("_______________________\nMédico Responsable")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(12))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            firmaTable.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph("_______________________\nSello de la Clínica")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(12))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            document.Add(new Paragraph("\n\n"));
            document.Add(firmaTable);

            document.Close();
        }

        // Open generated PDF
        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(filePath)
        });
    }
}
