namespace ProudCircleActivityBot; 

public interface IDataStorage {
    void Initialize();
    void InsertPlayer();
    void CleanUp();
}