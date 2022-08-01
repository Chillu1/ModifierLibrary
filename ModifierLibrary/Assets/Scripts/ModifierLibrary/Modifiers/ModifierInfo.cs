namespace ModifierLibrary
{
    public class ModifierInfo
    {
        public string DisplayName { get; }
        public string Description { get; }
        public string ModifierTextureId { get; }

        private string BaseInfo { get; }
        public string EffectInfo { get; set; } = "";
        public string BasicCheckInfo { get; set; } = "";
        public string BattleCheckInfo { get; set; } = "";
        

        private ICheckComponent _checkComponent;

        public ModifierInfo(string displayName, string description, string modifierTextureId = "")
        {
            DisplayName = displayName;
            Description = description;
            ModifierTextureId = modifierTextureId;

            BaseInfo = $"{DisplayName}\n";
        }

        public void Setup(ICheckComponent checkComponent)
        {
            _checkComponent = checkComponent;

            EffectInfo = _checkComponent.Info;
            UpdateInfo();
        }

        public void Update(float dt)
        {
            UpdateInfo();
        }

        public string GetFullInfo()
        {
            return BaseInfo+EffectInfo+BasicCheckInfo;
        }

        public void UpdateInfo()
        {
            if(_checkComponent == null)
                return;

            BasicCheckInfo = _checkComponent.GetBasicInfo();
            BattleCheckInfo = _checkComponent.GetBattleInfo();
        }

        public override string ToString()
        {
            return GetFullInfo();
        }
    }
}