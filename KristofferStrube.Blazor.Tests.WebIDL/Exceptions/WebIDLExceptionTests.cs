using FluentAssertions;
using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KristofferStrube.Blazor.Tests.WebIDL.Exceptions;

[TestClass]
public class WebIDLExceptionTests
{

    /// <summary>
    /// Tests that the <see cref="WebIDLException"/> can be constructed and that <see cref="WebIDLException.GetStackFrames" />
    /// correctly parses the stack.
    /// </summary>
    [TestMethod]
    public void WebIDLException_CanGetStackFrames_DeepStack()
    {
        var stacktrace = @"causeErrors@http://localhost:5010/myfunctions.js:7:9 
        window.onmessage@http://localhost:5010/:21:17
        EventHandlerNonNull*@http://localhost:5010/:18:9";

        WebIDLException exception = new("message", stacktrace);
        exception.Should().NotBeNull();
        exception.Message.Should().Be("message");
        exception.StackTrace.Should().Be(stacktrace);
        exception.GetStackFrames().Should().NotBeNullOrEmpty().And.HaveCount(3);
    }

    /// <summary>
    /// Tests that the <see cref="WebIDLException"/> can be constructed and that <see cref="WebIDLException.GetStackFrames" />
    /// returns an empty list when there is no stack.
    /// </summary>
    [TestMethod]
    public void WebIDLException_CanGetStackFrames_NoStack()
    {
        WebIDLException exception = new("message");
        exception.Should().NotBeNull();
        exception.Message.Should().Be("message");
        exception.StackTrace.Should().BeNull();
        exception.GetStackFrames().Should().NotBeNull().And.BeEmpty();
    }

}
