﻿(function () {
    Array.prototype.some = Array.prototype.some || function (fun/*, thisArg*/) {
        'use strict';

        if (this == null) {
            throw new TypeError('Array.prototype.some called on null or undefined');
        }

        if (typeof fun !== 'function') {
            throw new TypeError();
        }

        var t = Object(this);
        var len = t.length >>> 0;

        var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
        for (var i = 0; i < len; i++) {
            if (i in t && fun.call(thisArg, t[i], i, t)) {
                return true;
            }
        }

        return false;
    };

    Number.isSafeInteger = Number.isSafeInteger || function (value) {
        return Number.isInteger(value) && Math.abs(value) <= Number.MAX_SAFE_INTEGER;
    };
})();