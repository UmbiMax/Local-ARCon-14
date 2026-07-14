command-list-langs-desc = Показывает языки, которыми владеет управляемая вами сущность.
command-list-langs-help = Использование: {$command}
command-saylang-desc = Отправляет сообщение на выбранном языке. Язык можно указать названием или номером в списке.
command-saylang-help = Использование: {$command} <идентификатор языка> <сообщение>. Пример: {$command} GalacticCommon «Привет, мир!». Пример: {$command} 1 «Привет, мир!»
command-language-select-desc = Выбирает текущий язык сущности по названию или номеру в списке.
command-language-select-help = Использование: {$command} <идентификатор языка>. Пример: {$command} 1. Пример: {$command} GalacticCommon
command-language-spoken = Доступны для речи:
command-language-understood = Доступны для понимания:
command-language-current-entry = {$id}. {$language} - {$name} (текущий)
command-language-entry = {$id}. {$language} - {$name}
command-language-invalid-number = Номер языка должен находиться в диапазоне от 0 до {$total}. Альтернативно используйте название языка.
command-language-invalid-language = Языка «{$id}» не существует либо вы не можете на нём говорить.
# Toolshed

command-description-language-add = Добавляет переданной сущности язык. Последние два аргумента определяют возможность говорить и понимать его.
command-description-language-rm = Удаляет у переданной сущности язык. Аргументы аналогичны language:add.
command-description-language-lsspoken = Перечисляет все языки, на которых может говорить сущность. Пример: «собственный язык: lsspoken»
command-description-language-lsunderstood = Перечисляет все языки, которые может понимать сущность. Пример: 'собственный язык: lssunderstood'
command-description-translator-addlang = Добавляет новый целевой язык в объект транслируемого переводчика. См. язык:добавить для подробностей.
command-description-translator-rmlang = Удаляет целевой язык из передаваемого по конвейеру объекта-переводчика. Подробности см. в разделе Language:rm.
command-description-translator-addrequired = Добавляет новый необходимый язык в объект транслируемого переводчика. Пример: 'ent 1234 переводчик:addrequired "GalacticCommon"'
command-description-translator-rmrequired = Удаляет необходимый язык из передаваемого по конвейеру объекта-переводчика. Пример: 'ent 1234 переводчик:rmrequired "GalacticCommon"'
command-description-translator-lsspoken = Перечисляет все разговорные языки для объекта-переводчика. Пример: 'ent 1234 переводчик:lsspoken'
command-description-translator-lsunderstood = Перечисляет все понятные языки для передаваемого по конвейеру объекта-переводчика. Пример: 'ent 1234 переводчик: lssunderstood'
command-description-translator-lsrequired = Перечисляет все необходимые языки для объекта транслируемого переводчика. Пример: 'ent 1234 переводчик:lsrequired'
command-language-error-this-will-not-work = Это не сработает.
command-language-error-not-a-translator = Сущность {$entity} не является переводчиком.
