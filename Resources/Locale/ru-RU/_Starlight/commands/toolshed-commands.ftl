command-description-radio-addcustom =
    Добавляет пользовательский канал указанному компоненту переданной сущности. Последний аргумент определяет, следует ли создать отсутствующий компонент.
command-description-radio-remcustom =
    Удаляет пользовательский канал с заданным идентификатором из указанного компонента переданной сущности.
command-description-container-insertentity =
    Вставляет заданный объект в указанный контейнер в передаваемом объекте.
command-description-container-insert =
    Вставляет переданные по конвейеру объекты в указанный контейнер указанного объекта.
command-description-container-create =
    Создает новый контейнер в передаваемом объекте.
command-description-container-createslot =
    Создаёт новый контейнер-слот в переданной сущности.
command-description-container-delete =
    Удаляет контейнер в конвейерном объекте.
command-description-container-drop =
    Извлекает всё содержимое указанного контейнера переданной сущности.
command-description-container-dropandget =
    Извлекает всё содержимое указанного контейнера и возвращает извлечённые сущности вместо исходной.
command-description-container-dropanddelete =
    Удаляет все содержащиеся объекты из указанного контейнера в передаваемом по конвейеру объекте, а затем удаляет контейнер.
command-description-container-get =
    Получает объект-контейнер с заданным идентификатором контейнера в передаваемом объекте.
command-description-container-getentities =
    Получает все объекты в заданном контейнере в передаваемом объекте.
command-description-container-getcontaining =
    Возвращает все контейнеры, в которых находится переданная сущность.
command-description-container-getoutercontainer =
    Получает самый внешний контейнер, содержащий переданный по конвейеру объект.
command-description-container-getowner =
    Получает сущность, владеющую указанным контейнером.
command-description-solution-adjcapacity =
    Изменяет вместимость указанного раствора.
command-description-solution-adjtemperature =
    Регулирует температуру данного раствора.
command-description-solution-adjthermalenergy =
    Регулирует тепловую энергию данного раствора.
command-description-solution-create =
    Создаёт в переданной сущности новый раствор с заданным именем. Если такой раствор уже существует, возвращает его.
command-description-solution-delete =
    Удаляет указанный раствор из переданной сущности.
### Starlight (upstream #39080)
command-description-subtlemessage =
    Отправляет скрытое сообщение всем входным сущностям.
command-description-grid-getplayers =
    Возвращает всех игроков на переданных гридах.
command-description-grid-get =
    Возвращает гриды, на которых находятся переданные игроки.
command-description-grid-getstation =
    Возвращает станции, на которых находятся переданные игроки, либо станцию переданного грида.
command-description-crewmanifest-addto =
    Добавляет передаваемый объект в манифест экипажа указанной станции.
command-description-crewmanifest-removefrom =
    Удаляет передаваемый объект из манифеста экипажа указанной станции.
command-description-crewmanifest-addplayer =
    Добавляет указанного игрока в списки экипажа переданных по конвейеру станций.
command-description-crewmanifest-removeplayer =
    Удаляет указанного игрока из списков экипажа переданных по конвейеру станций.
command-description-storage-reshape =
    Изменяет форму хранилища на основе данных, созданных командой box2iconstructor.
command-description-box2iconstructor-new =
    Создайте новое определение списка Box2i для объекта, объедините его с командами box2iconstructor:add, а затем выполните команду, которая этого требует.
command-description-box2iconstructor-add =
    Добавьте новый Box2i к существующему определению. Прежде чем использовать это, вызовите box2iconstructor:new.
command-description-box2iconstructor-clean =
    Очистите неиспользуемое определение списка Box2i в объекте.
command-description-vector2dataconstructor-new =
    Создайте новое определение списка Vector2 для объекта, объедините его с командами Vector2dataconstructor:add, а затем выполните команду, которая этого требует.
command-description-vector2dataconstructor-add =
    Добавьте новый Vector2 к существующему определению. Прежде чем использовать это, вызовите вектор2dataconstructor:new.
command-description-vector2dataconstructor-clean =
    Очистите неиспользуемое определение списка Vector2 в объекте.
command-description-job-set =
    Изменяет должность переданной сущности.
command-description-job-delset =
    Изменяет должность переданной сущности, предварительно удаляя старую, чтобы повторно показать брифинг.
command-description-ccomp-ensure =
    Гарантирует, что все клиенты добавляют компонент с указанным именем в сущность, если она существует.
command-description-ccomp-write =
    Попытайтесь заставить всех клиентов записать что-нибудь в клиентский компонент.
command-description-ccomp-rm =
    Гарантирует, что все клиенты удалят компонент с указанным именем из сущности, если она существует.
command-description-globalsound-play =
    Глобально воспроизводит звук для переданных сущностей или игровых сессий.
command-description-polymorph-proto =
    Преобразовывает объект в указанный прототип полиморфа.
command-description-polymorph-begin =
    Маркер, начинающий последовательность инструкций по настройке полиморфа, прикрепляет к объекту PolymorphSetupComponent.
command-description-polymorph-setproto =
    Установите прототип, в который будет трансформироваться сущность.
command-description-polymorph-seteffect =
    Задаёт прототип, появляющийся поверх превращённой сущности. Обычно используется для визуальных эффектов.
command-description-polymorph-setdelay =
    Установите время ожидания в секундах, прежде чем можно будет снова активировать этот конкретный полиморф.
command-description-polymorph-setduration =
    Установите продолжительность действия полиморфа в секундах, прежде чем он автоматически вернется в исходное состояние.
command-description-polymorph-setforced =
    Определяет, может ли сущность самостоятельно активировать или отменить превращение.
command-description-polymorph-settransferdamage =
    Устанавливается для переноса урона от текущего объекта к полиморфированному объекту.
command-description-polymorph-settransfername =
    Установите, чтобы полиморфированная сущность наследовала имя оригинала.
command-description-polymorph-settransferappearance =
    Установите, следует ли переносить такие параметры, как волосы, цвет кожи, рост и т. д., в полиморфный объект.
command-description-polymorph-setinventory =
    Определяет способ переноса инвентаря сущности при превращении.
command-description-polymorph-setrevertoncrit =
    Определяет, отменяется ли превращение при переходе сущности в критическое состояние.
command-description-polymorph-setrevertondeath =
    Определяет, отменяется ли превращение после смерти сущности.
command-description-polymorph-setrevertondelete =
    Определяет, отменяется ли превращение при удалении сущности.
command-description-polymorph-setrevertoneat =
    Определяет, отменяется ли превращение, когда сущность съедают.
command-description-polymorph-setallowrepeats =
    Установите, разрешать ли повторяющиеся полиморфы или нет.
command-description-polymorph-setignoreallowrepeats =
    Разрешает полиморф, даже если настройка AllowRepeatedMorphs запрещает повторные превращения.
command-description-polymorph-setcooldown =
    Задаёт время перезарядки превращения в секундах.
command-description-polymorph-setentersound =
    Установите звук, который воспроизводится при входе в полиморф.
command-description-polymorph-setexitsound =
    Установите звук, который воспроизводится при выходе из полиморфа.
command-description-polymorph-clearentersound =
    Убрать звук, который воспроизводится при входе в полиморф.
command-description-polymorph-clearexitsound =
    Убрать звук, который воспроизводится при выходе из полиморфа.
command-description-polymorph-setenterpopup =
    Установите всплывающее окно, которое появляется при входе в полиморф.
command-description-polymorph-setexitpopup =
    Установите всплывающее окно, которое появляется при выходе из полиморфа.
command-description-polymorph-clearcopycomp =
    Очистите список компонентов для копирования в полиморф.
command-description-polymorph-addcopycomp =
    Добавьте запись в список компонентов для копирования в полиморф.
command-description-polymorph-rmcopycomp =
    Удалите запись из списка компонентов для копирования в полиморф.
command-description-polymorph-apply =
    Мгновенно примените полиморф и закончите.
command-description-polymorph-applyget =
    Мгновенно примените полиморф и завершите операцию, вернув новую сущность.
command-description-polymorph-addaction =
    Добавляет сущности действие полиморфа из текущей цепочки настроек. После этого следует вызвать polymorph:apply или polymorph:finish.
command-description-polymorph-addactionproto =
    Добавьте к сущности прототип действия полиморфа.
command-description-polymorph-rmaction =
    Удаляет у сущности действие полиморфа, добавленное через polymorph:addaction.
command-description-polymorph-rmactionproto =
    Удалите прототип действия полиморфа из сущности.
command-description-polymorph-revert =
    По возможности возвращает сущность в предыдущую форму.
command-description-polymorph-reset =
    Сбросьте полиморф объекта в исходное состояние.
command-description-polymorph-finish =
    Отмечает эту цепочку установки полиморфа как завершенную, очищая и удаляя компонент.
command-description-vv-open =
    Откройте окно ViewVariables передаваемого объекта или пути.
command-description-vv-write =
    Измените значение пути с помощью VV (просмотр переменных). В качестве значения можно использовать переменную, но это должна быть сериализованная строка.
command-description-vv-owrite =
    Измените значение пути с помощью VV (просмотр переменных). В качестве значения можно использовать необработанную переменную.
command-description-vv-read =
    Выводит значение по указанному пути через VV (просмотр переменных).
command-description-vv-rsave =
    Получите значение пути с помощью VV (просмотр переменных). Можно сохранить в переменную.
command-description-vv-rsaveraw =
    Получите значение пути с помощью VV (просмотр переменных). Можно сохранить в переменную. Сохраняет необработанное значение вместо сериализованной строки.
command-description-mind-wipe =
    Стирает разум игрока или сущности. После этого сущностью нельзя будет управлять, пока ей не назначат новый разум.
command-description-mind-takeover =
    Передаёт управление целевой сущностью, при необходимости создавая для неё разум и делая её разумной.
command-description-mind-takeoverwipe =
    Стирает текущий разум, а затем передаёт управление сущностью. Все роли и цели прежнего разума будут удалены.
command-description-mind-controlwipe =
    Сотрите разум целевого игрока и заставьте его контролировать передаваемую по каналу сущность, создав новый разум и сделав сущность разумной.
command-description-killsign-set =
    Примените сигнал уничтожения к объекту, используя указанное состояние.
command-description-killsign-list =
    Выводит список доступных сигналов уничтожения.
command-description-killsign-rm =
    Удалить сигнал уничтожения с объекта
command-description-fixinput =
    Обновляет входной контекст сеанса сущности.
command-description-faction-add =
    Добавьте фракцию к этой сущности.
command-description-faction-remove =
    Удалить фракцию из этой сущности.
command-description-faction-aggro =
    Сделайте эту сущность агрессивной по отношению к целевой сущности.
command-description-faction-deaggro =
    Снимает агрессию этой сущности по отношению к цели.
command-description-faction-ignore =
    Заставьте эту сущность и целевую сущность игнорировать друг друга.
command-description-faction-unignore =
    Заставьте эту сущность и целевую сущность больше не игнорировать друг друга.
command-description-faction-clear =
    Очистите фракции этой сущности.
command-description-npc-sethtn =
    Создает NPC для объекта и устанавливает для него состав HTN.
command-description-npc-setenabled =
    Включите или отключите поведение HTN этого NPC.
command-description-stationinit-begin =
    Начинает инициализацию новой станции в середине раунда. Добавляет гриду BecomesStationMidRoundComponent.
command-description-stationinit-setid =
    Установите идентификатор станции. Это сделано для предотвращения дублирования.
command-description-stationinit-clearbaseprotos =
    Очистите список прототипов базовых станций.
command-description-stationinit-addbaseproto =
    Добавьте прототип базовой станции для использования.
command-description-stationinit-rmbaseproto =
    Удалить прототип базовой станции из использования.
command-description-stationinit-setallowftl =
    Определяет, разрешены ли FTL-перелёты к карте, на которой находится станция.
command-description-stationinit-setuseemergencyshuttle =
    Определяет, следует ли создать эвакуационный шаттл для завершения раунда.
command-description-stationinit-setusearmories =
    Определяет, следует ли создать арсеналы, которые можно отправить на станцию соответствующей командой.
command-description-stationinit-setusearrivals =
    Определяет, следует ли создать шаттл прибытия для этой станции.
command-description-stationinit-setallowdungeonspawns =
    Определяет, разрешено ли появление данжей наподобие VGroid.
command-description-stationinit-setallowcargo =
    Определяет, разрешено ли появление грузовых шаттлов и ATS.
command-description-stationinit-clearallowedgridspawns =
    Очистите список сеточных спавнов, которым разрешено появляться из базовых прото.
command-description-stationinit-addallowedgridspawn =
    Добавьте сетку, которой разрешено появляться из базовых прототипов.
command-description-stationinit-rmallowedgridspawn =
    Удалите сетку, которой разрешено появляться из базовых прототипов.
command-description-stationinit-setemergencyshuttlepath =
    Задаёт переопределённый путь к гриду эвакуационного шаттла.
command-description-stationinit-clearjobs =
    Удаляет все должности на станции.
command-description-stationinit-addjob =
    Добавляет новую должность на станцию.
command-description-stationinit-rmjob =
    Удаляет должность со станции.
command-description-stationinit-setallowevents =
    Установите разрешение событий на эту станцию.
command-description-stationinit-setdovariationpass =
    Установите, позволяющее запускать вариационный проход в начале раунда на вновь созданной станции.
command-description-stationinit-namegrid =
    Переименуйте целевую сетку. Имя сетки будет использоваться в качестве имени станции при инициализации.
command-description-stationinit-initialize =
    Завершите настройку и инициализируйте станцию.
command-description-stationinit-initializeget =
    Завершите настройку и инициализируйте станцию ​​и верните вновь созданный объект станции.
command-description-aitakeover =
    Передаёт целевое ядро ИИ под управление переданной сущности.
command-description-mobthreshold-initialize =
    Правильно инициализирует новый порог моба для объекта.
command-description-corporeal-on =
    Делает вашего призрака видимым и дает ему возможность говорить.
command-description-corporeal-off =
    Делает вашего призрака невидимым и лишает способности говорить.
command-description-markup-adddesc =
    Добавьте текст разметки в описание передаваемого объекта с заданным идентификатором.
command-description-markup-editdesc =
    Отредактируйте строку текста разметки из описания передаваемого объекта с заданным идентификатором.
command-description-markup-rmdesc =
    Удалите строку текста разметки из описания передаваемого объекта с заданным идентификатором.
command-description-markup-cleardesc =
    Удаляет все дополнительные строки текста разметки из описания передаваемого объекта.
command-description-markup-listdesc =
    Перечисляет все тексты разметки описания передаваемого объекта и их идентификаторы.
command-description-atmos-add =
    Добавляет атмосферу трубопроводной сетке.
command-description-atmos-fix =
    Исправьте атмосферу трубопроводной сети.
command-description-atmos-rejoin =
    Попытайтесь заставить атмосферное устройство воссоединиться с атмосферой.
command-description-jobs-makeunlimited =
    Делает число мест на должности неограниченным.
command-description-jobs-makelimited =
    Ограничивает число мест на должности. Позволяет сбросить значение до нуля или текущего значения счётчика середины раунда.
