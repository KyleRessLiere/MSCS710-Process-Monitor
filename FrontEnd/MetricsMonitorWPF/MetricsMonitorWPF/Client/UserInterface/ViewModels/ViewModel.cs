// ViewModel.cs
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ViewModel : BaseViewModel {
    public ViewModel(
      IEnumerable<string> people,
      Action<int> handleRatingFeedback,
      Action<string> handleFavoritePerson) {
        RatingScale = new ObservableCollection(
          Enumerable
          .Range(1, 5)
          .Select(rating => new ButtonViewModel {
              Label = rating.ToString(),
              Command = new ActionCommand(() => handleRatingFeedback(rating))
          })
        );
        People = new ObservableCollection(
          people
          .Select(person => new ButtonViewModel {
              Label = person,
              Command = new ActionCommand(() => handleFavoritePerson(person))
          })
        );
    }

    public ObservableCollection<ButtonViewModel> RatingScale { get; }
    public ObservableCollection<ButtonViewModel> People { get; }
}