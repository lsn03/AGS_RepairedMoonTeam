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
