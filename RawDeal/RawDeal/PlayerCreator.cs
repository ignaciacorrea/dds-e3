using RawDealView;

namespace RawDeal;

public class PlayerCreator
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly List<Superstar> _listOfSuperstars;
    private List<Card> _playerDeck;
    private Superstar _playerSuperstar;
    
    public PlayerCreator(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _listOfSuperstars = DataLoader.LoadSuperstarData();
    }


    public Player SetUpPlayer()
    {
        string[] selectedDeckData = GetUserSelectedDeck();
        CreateUserSuperstar(selectedDeckData);
        CreateUserCards(selectedDeckData);
        if (!CheckIfPlayerCanBeCreated())
        {
            throw new InvalidOperationException("No se pudo crear el jugador porque el mazo es inválido.");
        }
        return CreatePlayer();
    }
    
    
    private string[] GetUserSelectedDeck()
    {
        string selectedDeckPath = _view.AskUserToSelectDeck(_deckFolder);
        string[] selectedDeckData = File.ReadAllLines(selectedDeckPath);
        return selectedDeckData;
    }
    
    
    private void CreateUserSuperstar(string[] selectedDeckData)
    {
        CleanUpDeckData(selectedDeckData);
        _playerSuperstar = _listOfSuperstars.FirstOrDefault(superstar => superstar.Name == selectedDeckData[0]);
    }


    private static void CleanUpDeckData(string[] selectedDeckData)
    {
        selectedDeckData[0] = selectedDeckData[0].Replace(" (Superstar Card)", "");
    }
    
    
    private void CreateUserCards(string[] selectedDeckData)
    {
        selectedDeckData = selectedDeckData.Skip(1).ToArray();
        CardCatalog catalogOfCards = new CardCatalog();
        _playerDeck = selectedDeckData.Select(title => catalogOfCards.GetCard(title)).ToList();
    }


    private bool CheckIfPlayerCanBeCreated()
    {
        DeckValidator validator = new DeckValidator(_playerDeck, _playerSuperstar, _listOfSuperstars);
        return validator.CheckIfDeckIsValid();
    }


    private Player CreatePlayer()
    {
        // SuperstarAbility userSuperstarAbility = DeterminateSuperstar(_playerSuperstar.Name);
        return new Player(_playerSuperstar, _playerDeck);
    }

    
    // private SuperstarAbility DeterminateSuperstar(string superstarName)
    // {
    //     return superstarName switch
    //     {
    //         "CHRIS JERICHO" => new ChrisJerichoAbility(_view),
    //         "THE UNDERTAKER" => new TheUndertakerAbility(_view),
    //         "STONE COLD STEVE AUSTIN" => new StoneColdSteveAustinAbility(_view),
    //         "THE ROCK" => new TheRockAbility(_view),
    //         "KANE" => new KaneAbility(_view),
    //         "MANKIND" => new MankindAbility(_view),
    //         _ => throw new ArgumentException("Superstar no reconocido: " + superstarName)
    //     };
    // }
}