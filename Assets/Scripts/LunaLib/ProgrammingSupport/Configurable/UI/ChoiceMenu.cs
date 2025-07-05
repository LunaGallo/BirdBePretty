using System;

namespace LunaLib {
    public class ChoiceMenu : CustomButtonPoolConfigurator<MenuChoiceButton, ChoiceMenuConfiguration, IconAndLabelButtonConfiguration> { }
    public class ChoiceMenuConfiguration : CustomButtonPoolConfiguration<IconAndLabelButtonConfiguration> { }
}