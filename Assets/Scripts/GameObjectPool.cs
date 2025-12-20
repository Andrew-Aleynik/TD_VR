using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly int _maxSize;
    private readonly List<GameObject> _objects = new List<GameObject>();

    public GameObjectPool(int maxSize = 100)
    {
        _maxSize = Mathf.Max(1, maxSize); // минимум 1
    }

    // Попытка добавить объект в пул. Возвращает true, если успешно.
    public bool Add(GameObject obj)
    {
        if (obj == null) 
            return false;

        // Удаляем все null-ссылки и "мёртвые" объекты перед добавлением
        Cleanup();

        // Если пул заполнен — не добавляем
        if (_objects.Count >= _maxSize)
            return false;

        _objects.Add(obj);
        return true;
    }

    // Текущее количество живых объектов (без null)
    public int Count
    {
        get
        {
            Cleanup();
            return _objects.Count;
        }
    }

    // Максимальный размер пула
    public int MaxSize => _maxSize;

    // Уничтожить все объекты и очистить пул
    public void DestroyAll()
    {
        foreach (var obj in _objects)
        {
            if (obj != null)
                GameObject.Destroy(obj);
        }
        _objects.Clear();
    }

    // Внутренний метод: удаляет null и уничтоженные объекты
    private void Cleanup()
    {
        for (int i = _objects.Count - 1; i >= 0; i--)
        {
            if (_objects[i] == null)
                _objects.RemoveAt(i);
        }
    }
}