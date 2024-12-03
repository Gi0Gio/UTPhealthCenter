using iText.IO.Image;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using System.Net;
using iText.Layout;

namespace HealthCare.Views.Doctors;

public partial class Patients : ContentPage
{
	public Patients()
	{
		InitializeComponent();
	}

    private async void Pdf(object sender, EventArgs e)
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "CertificadoBS.pdf");
        using (PdfWriter writer = new PdfWriter(filePath))
        {
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            
            string logoClinicaPath = "C:\\Users\\Gio\\Desktop\\All\\code\\UTPhealthCenter\\Resources\\Images\\logoclinica.png";
            string logoUniversidadPath = "C:\\Users\\Gio\\Desktop\\All\\code\\UTPhealthCenter\\Resources\\Images\\logouniver.png";

            iText.Layout.Element.Image logoClinica = new iText.Layout.Element.Image(ImageDataFactory.Create(logoClinicaPath)).SetWidth(100);
            iText.Layout.Element.Image logoUniversidad = new iText.Layout.Element.Image(ImageDataFactory.Create(logoUniversidadPath)).SetWidth(100);

            
            Table headerTable = new Table(new float[] { 1, 2, 1 }).UseAllAvailableWidth();

           
            headerTable.AddCell(new iText.Layout.Element.Cell().Add(logoClinica)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));

            
            Paragraph encabezado = new Paragraph("Universidad Tecnol�gica de Panama\nClinica HealthCare")
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

            string fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            string nombrePaciente = "Giovany Jovanne"; 
            string numeroIdentificacion = "123456789";  
            string nombreMedico = "Dr. Ana Gomez";  

          
            Paragraph fechaLugar = new Paragraph($"Fecha: {fechaActual}\nLugar: David, Chiriqu�")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                .SetFontSize(12);
            document.Add(fechaLugar);

            document.Add(new Paragraph("\n\n\n"));

            Paragraph contenido = new Paragraph($"Por la presente, hago constar que el/la estudiante {nombrePaciente}, " +
                $"con n�mero de identificaci�n {numeroIdentificacion}, ha sido evaluado/a en esta instituci�n " +
                "y se encuentra en buen estado de salud.\n\n" +
                "Certifico que el/la paciente no presenta ninguna condici�n de salud que le impida realizar " +
                "actividades f�sicas o laborales normales y no presenta signos de enfermedades contagiosas " +
                "que puedan representar un riesgo para otros.")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
            document.Add(contenido);

            document.Add(new Paragraph("\n\n\n\n\n\n\n"));

            Table firmaTable = new Table(2).UseAllAvailableWidth();

            iText.Layout.Element.Cell firmaMedico = new iText.Layout.Element.Cell().Add(new Paragraph("_______________________\nM�dico " + nombreMedico)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(12))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            firmaTable.AddCell(firmaMedico);

            iText.Layout.Element.Cell selloMedico = new iText.Layout.Element.Cell().Add(new Paragraph("_______________________\n[Sello del M�dico o Cl�nica]")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(12))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            firmaTable.AddCell(selloMedico);

            document.Add(firmaTable);

            document.Close();
        }

        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(filePath)
        });

    }

}