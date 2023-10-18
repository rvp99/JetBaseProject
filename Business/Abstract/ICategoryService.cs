using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract;

public interface ICategoryService
{
    IDataResult<List<Category>> GetAllCategory();
}