using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FilesController : ControllerBase
  {
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
      _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                                          ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }

    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
      var pathToFile = "Public/Coursera PMR5B596XWMP.pdf";

      if (!System.IO.File.Exists(pathToFile))
      {
        return NotFound();
      }

      if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile
            , out var contentType))
      {
        contentType = "application/octet-stream";
      }

      var bytes = System.IO.File.ReadAllBytes(pathToFile);
      return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
  }
}