// namespace RawDeal;
//
// public class TurnManager
// {
//     private Player _currentPlayer;
//     private Player _otherPlayer;
//     private SuperstarAbility _currentPlayerSuperstarAbility;
//     private SuperstarAbility _otherPlayerSuperstarAbility;
//     
//     public TurnManager(Player player1, Player player2)
//     {
//         _currentPlayer = player1;
//         _otherPlayer = player2;
//     }
//     
//     private void HandleTurn()
//     {
//         _currentPlayer.GetSuperstarAbility().UseAbilityBeforeDrawing();
//         _view.SayThatATurnBegins(_currentPlayer.GetSuperstarName());
//         StealCard();
//         PresentPlayersStatusAndPromptForAction();
//     }
// }