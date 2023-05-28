export function getAttribute(object, attribute) { return object[attribute]; }

export function forEachWithNoArguments(jSReference, callbackObjRef) {
    jSReference.forEach(() => callbackObjRef.invokeMethodAsync('InvokeCallback'))
}

export function forEachWithOneArgument(jSReference, callbackObjRef) {
    jSReference.forEach((value) => callbackObjRef.invokeMethodAsync('InvokeCallback', DotNet.createJSObjectReference(value)))
}

export function forEachWithTwoArguments(jSReference, callbackObjRef) {
    jSReference.forEach((value, key) => callbackObjRef.invokeMethodAsync('InvokeCallback', DotNet.createJSObjectReference(value, key)))
}

// https://javascriptweblog.wordpress.com/2011/08/08/fixing-the-javascript-typeof-operator/
export function valuePropertiesType(obj, attribute) {
    return ({}).toString.call(obj[attribute]).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
}

export function valuePropertiesValue(obj, attribute) {
    return obj[attribute];
}

export function constructUint8Array(argument) {
    return new Uint8Array(argument);
}

export function constructFloat32Array(argument) {
    return new Float32Array(argument);
}

export function constructDomException(message, name) {
    return new DOMException(message, name);
}

export function constructEvalError(message) {
    return EvalError(message);
}

export function constructRangeError(message) {
    return RangeError(message);
}

export function constructReferenceError(message) {
    return ReferenceError(message);
}

export function constructTypeError(message) {
    return TypeError(message);
}

export function constructURIError(message) {
    return URIError(message);
}

export async function callAsyncGlobalMethod(identifier, args) {
    return await callAsyncInstanceMethod(window, identifier, args);
}

export async function callAsyncInstanceMethod(instance, identifier, args) {
    try {
        var [functionObject, functionInstance] = resolveFunction(instance, identifier);
        return await functionInstance.apply(functionObject, args);
    }
    catch (error) {
        throw new DOMException(formatError(error), "AbortError");
    }
}

export function callGlobalMethod(identifier, args) {
    return callInstanceMethod(window, identifier, args);
}

export function callInstanceMethod(instance, identifier, args) {
    try {
        var [functionObject, functionInstance] = resolveFunction(instance, identifier);
        return functionInstance.apply(functionObject, args);
    }
    catch (error) {
        throw new DOMException(formatError(error), "AbortError");
    }
}

function resolveFunction(instance, identifier)
{
    let identifierParts = identifier.split(".");
    var functionObject = instance;
    var functionInstance = instance[identifierParts[0]];
    for (let i = 1; i < identifierParts.length; i++) {
        if (functionInstance == undefined) {
            throw new ReferenceError(`Cannot read properties of undefined (reading '${identifierParts[i - 1]}').`);
        }
        functionObject = functionInstance;
        functionInstance = functionInstance[identifierParts[i]];
    }
    if (!(functionInstance instanceof Function)) {
        throw new TypeError(`'${identifierParts.slice(-1)}' is not a function.`);
    }
    return [functionObject, functionInstance];
}

function formatError(error) {
    var name = error.name;
    if (error instanceof DOMException && name == "SyntaxError") {
        name = "DOMExceptionSyntaxError";
    }
    return JSON.stringify({
        name: error.name,
        message: error.message,
        stack: error.stack
    })
}