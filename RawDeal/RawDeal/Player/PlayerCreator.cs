using RawDealView;

namespace RawDeal;

public class PlayerCreator
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly List<Superstar> _listOfSuperstars;
    private IEnumerable<Card> _playerDeck;
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
            throw new InvalidDeckException("No se pudo crear el jugador porque el mazo es inválido.");
        }
        return CreatePlayerInformation();
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
        _playerDeck = selectedDeckData.Select(title => catalogOfCards.GetCard(title));
    }


    private bool CheckIfPlayerCanBeCreated()
    {
        DeckValidator validator = new DeckValidator(_playerDeck, _playerSuperstar, _listOfSuperstars);
        return validator.CheckIfDeckIsValid();
    }


    private Player CreatePlayerInformation()
    {
        Player player = new Player(_playerSuperstar, _playerDeck);
        player.SuperstarAbility = CreateSuperstarAbility(player);
        return player;
    }

    
    public SuperstarAbility CreateSuperstarAbility(Player player)
    {
        return player.GetSuperstarName() switch
        {
            // "CHRIS JERICHO" => new ChrisJerichoAbility(_view),
            // "THE UNDERTAKER" => new TheUndertakerAbility(_view),
            // "STONE COLD STEVE AUSTIN" => new StoneColdSteveAustinAbility(_view),
            "THE ROCK" => new TheRockAbility(_view, player),
            // "HHH" => new HhhAbility(_view),
            "KANE" => new KaneAbility(_view, player),
            "MANKIND" => new MankindAbility(_view, player),
            _ => new SuperstarAbility(),
        };
    }
}