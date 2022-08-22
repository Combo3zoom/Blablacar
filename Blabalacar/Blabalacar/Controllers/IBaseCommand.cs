using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;

public interface IBaseCommand<T> where T: class
{

    //public Enumerable<T> Get();
    public IActionResult Get(int id);
    public IActionResult Post(T changing);
    public IActionResult Put(T changing);
    public IActionResult Delete(int id);
}