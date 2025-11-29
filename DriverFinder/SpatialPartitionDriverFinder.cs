using DriverFinder;

public class SpatialPartitionDriverFinder : IDriverFinder
{
    // Сервис для работы с водителями
    private readonly IDriverService _driverService;

    // Размер ячейки сетки
    private readonly int _gridSize;

    // Сетка для хранения водителей (ключ - координаты ячейки, значение - список водителей)
    private Dictionary<(int, int), List<Driver>> _grid;

    // Название алгоритма
    public string AlgorithmName => "Spatial Partitioning";

    // Конструктор
    public SpatialPartitionDriverFinder(IDriverService driverService, int gridSize = 10)
    {
        // Сохраняем сервис водителей
        _driverService = driverService;

        // Сохраняем размер сетки
        _gridSize = gridSize;

        // Инициализируем сетку
        _grid = new Dictionary<(int, int), List<Driver>>();

        // Строим первоначальную сетку
        BuildGrid();
    }

    // Построение сетки
    private void BuildGrid()
    {
        // Очищаем текущую сетку
        _grid.Clear();

        // Проходим по всем водителям
        foreach (var driver in _driverService.GetAllDrivers())
        {
            // Вычисляем ключ ячейки (целочисленное деление координат на размер сетки)
            var gridKey = (driver.X / _gridSize, driver.Y / _gridSize);

            // Если ячейка не существует, создаем ее
            if (!_grid.ContainsKey(gridKey))
            {
                _grid[gridKey] = new List<Driver>();
            }

            // Добавляем водителя в ячейку
            _grid[gridKey].Add(driver);
        }
    }

    public List<DriverSearchResult> FindNearestDrivers(int targetX, int targetY, int count = 5)
    {
        // Перестраиваем сетку (на случай изменения водителей)
        BuildGrid();

        // Вычисляем ячейку цели
        var targetGridKey = (targetX / _gridSize, targetY / _gridSize);

        // Список кандидатов для поиска
        var candidates = new List<Driver>();

        // Ищем в целевой ячейке и соседних ячейках (3x3 область)
        for (int dx = -1; dx <= 1; dx++)  // Смещение по X
        {
            for (int dy = -1; dy <= 1; dy++)  // Смещение по Y
            {
                // Вычисляем ключ соседней ячейки
                var searchKey = (targetGridKey.Item1 + dx, targetGridKey.Item2 + dy);

                // Если ячейка существует, добавляем ее водителей в кандидаты
                if (_grid.ContainsKey(searchKey))
                {
                    candidates.AddRange(_grid[searchKey]);
                }
            }
        }

        // Если кандидатов меньше чем нужно, ищем среди всех водителей
        if (candidates.Count < count)
        {
            candidates = _driverService.GetAllDrivers().ToList();
        }

        // Сортируем кандидатов по расстоянию и берем ближайших
        return candidates
            .Select(d => new DriverSearchResult(d, d.DistanceTo(targetX, targetY)))  // Преобразуем в результаты
            .OrderBy(r => r.Distance)  // Сортируем по расстоянию
            .Take(count)  // Берем count ближайших
            .ToList();  // Преобразуем в список
    }
}