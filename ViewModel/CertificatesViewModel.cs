using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthCare.ViewModel;

public class CertificatesViewModel : BaseViewModel
{
    public ObservableCollection<StudyRequest> StudyRequests { get; set; }
    public ICommand GenerateHealthCertificateCommand { get; }
    public ICommand GenerateStudyPdfCommand { get; }

    public CertificatesViewModel()
    {
        StudyRequests = new ObservableCollection<StudyRequest>
        {
            new StudyRequest { Id = 1, Name = "Estudio de Sangre" },
            new StudyRequest { Id = 2, Name = "Radiografía" }
        };
        GenerateHealthCertificateCommand = new Command(GenerateHealthCertificate);
        GenerateStudyPdfCommand = new Command<int>(GenerateStudyPdf);
    }

    private async void GenerateHealthCertificate()
    {

    }

    private async void GenerateStudyPdf(int id)
    {

    }
}

public class StudyRequest
{
    public int Id { get; set; }
    public string UserName { get; set; }
}
