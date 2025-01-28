using LemonInc.Core.Input;

namespace Game.MainMenu
{
    public class MenuInputProvider : InputProviderBase<Controls>
    {
        public PhysicalInput RestartStage { get; } = new();
        public PhysicalInput SelectMenu { get; } = new();

        protected override void SubscribeInputs()
        {
            RestartStage.Subscribe(Controls.Other.RestartStage);
            SelectMenu.Subscribe(Controls.Other.SelectMenu);
        }

        protected override void UnSubscribeInputs()
        {
            RestartStage.UnSubscribe();
            SelectMenu.UnSubscribe();
        }
    }
}