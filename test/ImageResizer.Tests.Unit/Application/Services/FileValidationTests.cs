using System.Text;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Configuration;
using ImageResizer.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ImageResizer.Tests.Unit.Application.Services
{
    public class FileValidationTests
    {
        private readonly Mock<IOptions<ResizerSettings>> _resizerSettingsMock;
        private readonly Mock<ILogger<FileValidationService>> _loggerMock;
        private readonly Mock<IFormFile> _formFileMock;

        private readonly FileValidationService _fileValidationService;

        public FileValidationTests()
        {
            _resizerSettingsMock = new Mock<IOptions<ResizerSettings>>();
            _loggerMock = new Mock<ILogger<FileValidationService>>();

            _formFileMock = new Mock<IFormFile>();

            _fileValidationService = new FileValidationService(
                _resizerSettingsMock.Object,
                _loggerMock.Object
            );

            SetupMocks();
        }

        [Fact]
        public void ValidateImageForUpload_ParameterIsNull_ExceptionIsThrown()
        {
            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(null!));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateImageForUpload_FileNameIsNull_ArgumentNullExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.FileName).Returns((string)null!);

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_ContentTypeIsNull_ArgumentNullExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.ContentType).Returns((string)null!);

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<CustomHttpException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_ContentTypeIsUnsupported_CustomHttpExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.ContentType).Returns("text/plain");

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<CustomHttpException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_ExtensionIsUnsupported_CustomHttpExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.FileName).Returns("something.mp3");

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<CustomHttpException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_FileSizeIsZero_CustomHttpExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.Length).Returns(0);

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<CustomHttpException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_FileSizeIsLargerThanExpected_ExceptionIsThrown()
        {
            _formFileMock.Setup(f => f.Length).Returns(2 * 1024 * 1024);

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(_formFileMock.Object));

            Assert.NotNull(ex);
            Assert.IsType<CustomHttpException>(ex);
        }

        [Fact]
        public void ValidateImageForUpload_UploadedFileIsNotAnImage_CustomHttpExceptionIsThrown()
        {
            var mockFile = new Mock<IFormFile>();
            
            var fileContent = Encoding.UTF8.GetBytes("Mock file content");
            mockFile.Setup(f => f.FileName).Returns("something.jpg");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            mockFile.Setup(f => f.Length).Returns(123);
            
            var stream = new MemoryStream(fileContent);
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

            var ex = Record.Exception(() => _fileValidationService.ValidateImageForUpload(mockFile.Object));

            Assert.IsType<CustomHttpException>(ex);
            Assert.Contains("Unsupported content.", ex.Message);
        }

        private void SetupMocks()
        {
            var resizerSettings = new ResizerSettings()
            {
                ImageSettings = new Configuration.Models.ImageSettingsElement()
                {
                    SupportedExtensions = [".jpg"],
                    MaxFileSizeInMB = 1
                }
            };

            _resizerSettingsMock.Setup(x => x.Value).Returns(resizerSettings);

            _formFileMock.Setup(f => f.FileName).Returns("something.jpg");
        }
    }
}
