using Xunit;

public class FileValidationTests
{
    [Fact]
    public void Upload_InvalidFileType_ShouldFail()
    {
        // Arrange
        var fileName = "testing.exe";

        // Act
        var isValid = FileValidator.IsPdf(fileName);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Upload_PdfFile_ShouldPass()
    {
        // Arrange
        var fileName = "contract.pdf";

        // Act
        var isValid = FileValidator.IsPdf(fileName);

        // Assert
        Assert.True(isValid);
    }
}

// Helper class
public static class FileValidator
{
    public static bool IsPdf(string fileName)
    {
        return fileName.EndsWith(".pdf");
    }
}