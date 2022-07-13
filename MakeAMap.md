# AGS_RepairedMoonTeam

Создание карты

Все необходимое лежит в папке Prefabs/MapMaker
1. Создаем новую сцену
2. Убираем дефолтную Main Camera из иерархии
3. Заходим в MapMaker, перетаскиваем камеру
4. Перетаскиваем в иерархию наш фон(Assets/Sprites/Backgrounds), устанавливаем размеры, меняем Order in Layer на -1
5. Создаем 2д объект tilemap Rectangular
6. На созданный Grid заходим во внутрь и на Tilemap меняем layer с Default на Ground
7. На Tilemap меняем Order in Layer на 1, добавляем компонент Tilemap Collider 2D


Раскидывание объектов, спавн, пушки, бусты

Все в той же папке Prefabs/MapMaker
1. Перетаскиваем BoostManager 
  У нас существуют некоторые классы предметов, поэтому для удобства работы с ними создан Boost Manager
  1.1 Копируя CTRL+D объекты Heart/Armor мы увеличиваем количество текущих предметов
2. Аналогично JumpPad Manager
3. Спавн Игроков через SpawnManager
  3.1 Необходимо дублировать SpawnPoint. Белая оболчка нужна для вида и тестирования, во время запуска она пропадает
4. Переходим на уровень выше в папку GunManager
  4.1 Аналогично перетаскиваем GunManager
  4.2 Объекты для дублирования - Lazer,AutomaticRifle, RocketLauncher

Завершающие штрихи
1. В папке MapMaker перетаскиваем в иерархию MenuAndDeathCanvas
2. Меняем в Canvas поле Redner Camera с None на нашу камеру, которая в иерархии(перетаскиванием)
3. Если нет в иерархии EventSystem, то необъодимо его добавить. Он располагается в UI в самом низу

Добавление карты для выбора и в билд.

1. Открываем сцену Lobby
2. Canvas -> MapSelector. Для удобства можно сделать меню LoadingMenu неактивным.
3. Делаем MapSelector активным
4. Заходим в MapsContainer
5. Добавляем новую карту, дублируя существующий GameObject( GO ) или в Prefabs
6. В каждой карте есть свои настройки, хранящиеся в PropertiesOfMap. Number - номер карты в билде. Его указывать обязательно. Name - имя карты собственно, может быть каким угодно.
7. Переходим в Canvas в сцене Lobby
8. В инспекторе листаем вниз до скрипта Launcher. В поле Map через "+" добавляем карту
9. Имя может быть произвольное, но лучше копировать с сцены для удобства.
10. в поле Scene вписываем номер сцены и делаем оставляем активными только LoadingMenu,Background,ExitGameButton.

Добавление карты в Build

1. открываем папку Scenes
2. Ctrl+Shift+B либо левый верхний угол File -> BuildSettings
3. Перетаскиваем сцену из папки Scenes в поле Scenes in Build, сохраняя нумерацию
4. Ctrl+S