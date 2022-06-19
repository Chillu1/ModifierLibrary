namespace ModifierSystem
{
    public class ApplierModifierInfo
    {

    }

    public class ModifierInfo
    {
        public string DisplayName { get; }
        private string Description { get; }
        public string ModifierTextureId { get; }

        private string BaseInfo { get; set; }
        public string CheckInfo { get; set; } = "";

        private ICheckComponent _checkComponent;

        public ModifierInfo(string displayName, string description, string modifierTextureId = "")
        {
            DisplayName = displayName;
            Description = description;
            ModifierTextureId = modifierTextureId;

            BaseInfo = $"{DisplayName}\n{Description}\n";
        }

        public void Setup(ICheckComponent checkComponent)
        {
            _checkComponent = checkComponent;
        }

        public void Update(float dt)
        {
            UpdateInfo();
        }

        public string GetInfo()
        {
            //TODO EffectInfo, Damage, etc
            return BaseInfo+CheckInfo;
        }

        public void UpdateInfo()
        {
            if(_checkComponent == null)
                return;

            CheckInfo = _checkComponent.DisplayText();
        }

        public override string ToString()
        {
            return GetInfo();
        }
    }
}