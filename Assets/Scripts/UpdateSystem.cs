using System.Collections.Generic;
using UnityEngine;

/* Этот проект реализует кастомную систему обновления
 * Стандартные MonoBehavior вызывают событие Update каждый фрейм, и, каждый раз, происходит передача управления между управляемым кодом (то что внутри функции update) и неуправляемым (закрытый код юнити)
 * Эта система создает список объектов обновления и вызывает у них функцию OnUpdate и OnStart
 * 
 * 
 * 
 * 
 */
// IUpdatable интерфейс
public interface IUpdatable
{
    //Все объекты должны идентифицироваться как объекты обновления, для этого создан интерфейс, который наследуют все подобные компоненты
    //У них у всех должна быть функция OnUpdate
    void OnUpdate(float dt);
    void OnLateUpdate(float dt);
    void OnFixedUpdate(float dt);
}
public class UpdateSystemBehaviour : MonoBehaviour, IUpdatable
{
    // Когда объект появляется
    void Awake()
    {
        // он регистрирует себя в менеджере
        UpdateSystem.Instance.Register(this);
        OnAwake();
    }

    void Start() => OnStart();

    // Когда объект уничтожается, он убирается из списка
    void OnDestroy() => UpdateSystem.Instance.Delete(this);

    // Виртуальная функция OnStart
    protected virtual void OnStart()
    {
        // Вызывается самим объектом
    }

    // Виртуальная функция OnAwake
    protected virtual void OnAwake()
    {
        // Вызывается самим объектом
    }

    // Виртуальная функция OnUpdate
    public virtual void OnUpdate(float dt)
    {
        // Вызывается менеджером
    }

    // Виртуальная функция OnFixedUpdate
    public virtual void OnFixedUpdate(float dt)
    {
        // Вызывается менеджером
    }

    public virtual void OnLateUpdate(float dt)
    {
        // Вызывается менеджером
    }
}
public class UpdateSystem : MonoBehaviour
{
    // Менеджер - синглтон
    public static UpdateSystem Instance { get; private set; }

    // Список объектов для обновления
    public List<UpdateSystemBehaviour> itemsToUpdate = new List<UpdateSystemBehaviour>();

    void Awake()
    {
        // Определить синглтон
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    /// <summary>
    /// Зарегистрировать объект обновления
    /// </summary>
    /// <param name="mono">компонент</param>
    public void Register(UpdateSystemBehaviour mono)
    {
        if (!itemsToUpdate.Contains(mono))
            itemsToUpdate.Add(mono);
    }

    /// <summary>
    /// Удалить компонент обновления
    /// </summary>
    /// <param name="mono">компонент</param>
    public void Delete(UpdateSystemBehaviour mono)
    {
        if (itemsToUpdate.Contains(mono))
            itemsToUpdate.Remove(mono);
    }

    float dt;
    void Update()
    {
        // Сохранить deltaTime
        dt = Time.deltaTime;
        // Вызов функций обновления на элементах списка
        for (int i = 0; i < itemsToUpdate.Count; i++)
            itemsToUpdate[i].OnUpdate(dt);
    }

    void FixedUpdate()
    {
        // Вызов функций фикс. обновления на элементах списка
        for (int i = 0; i < itemsToUpdate.Count; i++)
            itemsToUpdate[i].OnFixedUpdate(dt);
    }

    void LateUpdate()
    {
        // Вызов функций фикс. обновления на элементах списка
        for (int i = 0; i < itemsToUpdate.Count; i++)
            itemsToUpdate[i].OnLateUpdate(dt);
    }
}