job-no-requirements = Эта работа не имеет требований.
ghost-role-no-requirements = Эта роль не имеет никаких требований.

# Coloring rule of thumb: limegreen for met requirement, yellow for unmet requirement that can still be met, red for unmeetable

role-timer-department-sufficient = У вас [color=limegreen]{TOSTRING($current, "0")}[/color] из требуемых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры в отделе [color={$departmentColor}]{$department}[/color].
role-timer-department-not-too-high = У вас [color=limegreen]{TOSTRING($current, "0")}[/color] из максимально допустимых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры в отделе [color={$departmentColor}]{$department}[/color].
role-timer-department-insufficient = У вас [color=yellow]{TOSTRING($current, "0")}[/color] из требуемых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры в отделе [color={$departmentColor}]{$department}[/color].
role-timer-department-too-high = У вас [color=red]{TOSTRING($current, "0")}[/color] при максимально допустимых [color=lightblue]{TOSTRING($required, "0")}[/color] часах игры в отделе [color={$departmentColor}]{$department}[/color]. Возможно, вы выбираете роль стажёра.

role-timer-overall-sufficient = У вас есть [color=limegreen]{TOSTRING($current, "0")}[/color] из [color=lightblue]{TOSTRING($required, "0")}[/color] общего игрового времени.
role-timer-overall-not-too-high = У вас есть [color=limegreen]{TOSTRING($current, "0")}[/color] из не более [color=lightblue]{TOSTRING($required, "0")}[/color] общего времени игры.
role-timer-overall-insufficient = У вас [color=yellow]{TOSTRING($current, "0")}[/color] из требуемых [color=lightblue]{TOSTRING($required, "0")}[/color] часов общего игрового времени.
role-timer-overall-too-high = У вас [color=red]{TOSTRING($current, "0")}[/color] при максимально допустимых [color=lightblue]{TOSTRING($required, "0")}[/color] часах общего игрового времени. Возможно, вы выбираете роль стажёра.

role-timer-role-sufficient = У вас [color=limegreen]{TOSTRING($current, "0")}[/color] из требуемых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры на должности [color={$departmentColor}]{$job}[/color].
role-timer-role-not-too-high = У вас [color=limegreen]{TOSTRING($current, "0")}[/color] из максимально допустимых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры на должности [color={$departmentColor}]{$job}[/color].
role-timer-role-insufficient = У вас [color=yellow]{TOSTRING($current, "0")}[/color] из требуемых [color=lightblue]{TOSTRING($required, "0")}[/color] часов игры на должности [color={$departmentColor}]{$job}[/color].
role-timer-role-too-high = У вас [color=red]{TOSTRING($current, "0")}[/color] при максимально допустимых [color=lightblue]{TOSTRING($required, "0")}[/color] часах игры на должности [color={$departmentColor}]{$job}[/color]. Возможно, вы выбираете роль стажёра.

role-whitelisted = Вы [color=limegreen]находитесь[/color] в белом списке на эту роль.
role-not-whitelisted = Вы [color=yellow]не включены[/color] в белый список этой роли.
role-ban = Вам [color=red]запрещено[/color] занимать эту роль.

role-timer-age-old-enough = Чтобы играть эту роль, возраст вашего персонажа должен быть не ниже [color=limegreen]{$age}[/color].
role-timer-age-not-old-enough = Чтобы играть эту роль, возраст вашего персонажа должен быть не ниже [color=yellow]{$age}[/color].
role-timer-age-young-enough = Чтобы играть эту роль, возраст вашего персонажа должен быть не более [color=limegreen]{$age}[/color].
role-timer-age-not-young-enough = Чтобы играть эту роль, возраст вашего персонажа должен быть не более [color=yellow]{$age}[/color].

role-timer-whitelisted-species-pass = Чтобы играть эту роль, ваш персонаж [color=limegreen]должен[/color] принадлежать к одному из следующих видов: [color=limegreen]{$species}[/color]
role-timer-whitelisted-species-fail = Чтобы играть эту роль, ваш персонаж [color=yellow]должен[/color] принадлежать к одному из следующих видов: [color=yellow]{$species}[/color]
role-timer-blacklisted-species-pass = Чтобы играть эту роль, ваш персонаж [color=limegreen]не должен[/color] принадлежать к одному из следующих видов: [color=limegreen]{$species}[/color]
role-timer-blacklisted-species-fail = Чтобы играть эту роль, ваш персонаж [color=yellow]не должен[/color] принадлежать к одному из следующих видов: [color=yellow]{$species}[/color]

role-timer-whitelisted-traits-pass = Ваш персонаж [color=limegreen]должен[/color] иметь одну из следующих черт: [color=limegreen]{$traits}[/color]
role-timer-whitelisted-traits-fail = Ваш персонаж [color=yellow]должен[/color] иметь одну из следующих черт: [color=yellow]{$traits}[/color]
role-timer-blacklisted-traits-pass = Ваш персонаж [color=limegreen] не должен[/color] иметь одну из следующих черт: [color=limegreen]{$traits}[/color]
role-timer-blacklisted-traits-fail = Ваш персонаж [color=yellow] не должен[/color] иметь одну из следующих черт: [color=yellow]{$traits}[/color]
