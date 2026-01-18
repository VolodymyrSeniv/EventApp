using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models;
// using System.Web; // Zb?dne, je?li przekazujesz parametry jako s?ownik (Dictionary)

namespace MauiApp1.ViewModels;

// 1. Usu? [QueryProperty] - jest zb?dny przy IQueryAttributable
public partial class EventDetailViewModel : BaseViewModel, IQueryAttributable
{
    // To pole wygeneruje publiczn? w?a?ciwo?? "Eventik"
    [ObservableProperty]
    Event eventik;

    // To pole wygeneruje publiczn? w?a?ciwo?? "Id"
    [ObservableProperty]
    int id;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // 2. U?yj bezpiecznego klucza tekstowego "Id" zamiast nameof(Id),
        //    je?li kompilator ma problem z widoczno?ci? wygenerowanej w?a?ciwo?ci.
        if (query.ContainsKey("Id"))
        {
            // Odczytujemy ID z parametrów nawigacji
            int receivedId = Convert.ToInt32(query["Id"]);

            // Przypisujemy do naszej w?a?ciwo?ci
            id = receivedId;

            // Pobieramy dane z bazy
            Eventik = App.EventService.GetEvent(id);
        }
    }
}