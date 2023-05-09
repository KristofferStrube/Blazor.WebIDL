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
export function valuePropertiesType(obj) {
    return ({}).toString.call(obj.value).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
}

export function valuePropertiesValue(obj) {
    return obj.value;
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

export async function callGlobalAsyncMethod(identifier, args) {
    try {
        var functionObject = window;
        var functionInstance = window;

        let identifierParts = identifier.split(".");
        functionInstance = functionInstance[identifierParts[0]];
        for (let i = 1; i < identifierParts.length; i++) {
            functionObject = functionInstance;
            functionInstance = functionInstance[identifierParts[i]];
        }
        if (functionInstance instanceof Function) {
            return await functionInstance.apply(functionObject, args);
        }
        throw new ReferenceError(`Last part of the identifier '${identifier}' was not a function.`);
    }
    catch (error) {
        throw new DOMException(`${error.name}:${error.message}`, "AbortError");
    }
}