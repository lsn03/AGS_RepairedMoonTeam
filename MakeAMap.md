# AGS_RepairedMoonTeam
!!!!!!!!!!!!СОВЕТЫ!!!!!!!!!!!!!1
Чтобы не засорять иерархию, рекомендуется объединять предметы в пустые GameObjects
Таким образом, если в иерархии больше двух объектов света, то рекомендуется создать
пустой GameObject и перенести в него эти объекты. И так со всеми объектами.


Создание карты

Все необходимое лежит в папке Prefabs/MapMaker
1. Создаем новую сцену
2. Убираем дефолтную Main Camera из иерархии
3. Заходим в MapMaker, перетаскиваем камеру, распаковываем префаб Unpack Completely
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
  3.2 распаковываем префаб Unpack Completely
4. Переходим на уровень выше в папку GunManager
  4.1 Аналогично перетаскиваем GunManager
  4.2 Объекты для дублирования - Lazer,AutomaticRifle, RocketLauncher

Завершающие штрихи
1. В папке MapMaker перетаскиваем в иерархию MenuAndDeathCanvas, распаковываем префаб Unpack Completely
2. Меняем в Canvas поле Redner Camera с None на нашу камеру, которая в иерархии(перетаскиванием)
3. Если нет в иерархии EventSystem, то необходимо его добавить. Он располагается в UI в самом низу
5. Из той же папки перемещаем ScoreBoardCanvas и распаковываем префаб Unpack Completely
6. Меняем в Canvas поле Redner Camera с None на нашу камеру, которая в иерархии(перетаскиванием)

DeathMatch. Установка от А до Я.
1. MapMaker/SetupTeamAndTime
2. перетаскиваем CanvasTime, MenuAndDeathCanvas, ScoreBoardCanvas, EndGameCanvasDeathMatch.
3. Раскрываем CanvasTime, устанавливаем время.
4. У нас пустые поля EndGameCanvas,EndGameScoreBoard,GameManager,ScoreBoard
5. В поле EndGameCanvas перетаскиваем ScoreBoard из иерархии EndGameCanvasDeathMatch/ScoreBoard
6. В поле EndGameScoreBoard повторяем пункт 5.
7. В поле GameManager перетаскиваем MenuAndDeathCanvas.
8. В поле ScoreBoard перетаскиваем ScoreBoardCanvas/ScoreBoard
9. Открываем EndGameCanvasDeathMatch и в Render Camera устанавливаем нашу камеру на сцене
10. Для ScoreBoardCanvas повторяем пункт 9
11. Для MenuAndDeathCamvas повторяем пункт 9
12. MenuAndDeathCanvas в скрипте GameManager имеет пустое поле Button. Эта кнопка находится в EndGameCanvasDeathMatch/Leave Button (1). Устанавливаем поле Button перетаскиванием.

TeamDeathMatch. Установка от А до Я.
Хорошим исходником является TDMcyberfavels
1. MapMaker/SetupTeamAndTime
2. Перетаскиваем CanvasTime, MenuAndDeathCanvas, TeamScoreBoardCanvas, EndGameCanvasTeam
3. Установка аналогична пунктам 5,6,7,8,9,10,11,12.
4. Наше внимание теперь переключено на SpawnManager, точнее на его дочерние объекты SpawnPoint.
5. В зависимости от респавна (если синий респавн, то мы включаем режим blue, если красный - red, если у нас обычный DeathMatch, то оставляем поля Red и Blue пустыми.


Командный режим. Лидерборд.
1. MapMaker/TeamMode перетаскиваем EndGameCanvasDeathMatch,

9. Раскрываем ScoreBoardCanvas, видим Scoreboard. 
10. Выбираем EndGameCanvas(он должен быть распакован)
11. Меняем в Canvas поле Redner Camera с None на нашу камеру, которая в иерархии(перетаскиванием)
12. Открываем MenuAndDeathCanvas и перетаскиваем в поле Button кнопку, которая находится в EndGameCanvas под названием LeaveButton (1)
13. Открывает EndGameCanvas/ScoreBoard и ставим флажок Is End Game Canvas

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
![image](https://user-images.githubusercontent.com/85879960/183249323-c514dc70-1ef8-4a47-8070-bae3cfecb40e.png)
11. Выбираем EndGameCanvas, выбираем LeaveButton (1).
12. В событие On Click у нас None(Object), перетаскиваем туда MenuAndDeathCanvas
13. Выбираем No Function, в выпадающем списке выбираем GameManager, там выбираем Leave()
14. На объекте TimeManager устанавливаем длительность матча для каждой карты(задаем минуты и секунды)
Добавление карты в Build

1. открываем папку Scenes
2. Ctrl+Shift+B либо левый верхний угол File -> BuildSettings
3. Перетаскиваем сцену из папки Scenes в поле Scenes in Build, сохраняя нумерацию
4. Ctrl+S
