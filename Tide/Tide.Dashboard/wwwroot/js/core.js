
class CoreJS {
    constructor() {
        this.__object = null;
        this.__converter = null;
        this.__ajax = null;
        this.__swall = null;
        this.__validator = null;
        this.__loader = null;
        this.__math = null;
        this.__array = null;
        this.__dictionary = null;
        this.__text = null;
    }
    /*get object manipulation instance*/
    get object() {
        if (this.__object === null)
            this.__object = new ObjectManipulationJS();

        return this.__object;
    }
    get converter() {
        if (this.__converter === null)
            this.__converter = new ConverterService();
        return this.__converter;
    }
    get ajax() {
        if (this.__ajax === null)
            this.__ajax = new AjaxService();
        return this.__ajax;
    }

    get swall() {
        if (this.__swall === null)
            this.__swall = new SwallService();
        return this.__swall;
    }
    get validator() {
        if (this.__validator === null)
            this.__validator = new ValidatorService();
        return this.__validator;
    }
    get loader() {
        if (this.__loader === null)
            this.__loader = new LoaderService();
        return this.__loader;
    }
    get math() {
        if (this.__math === null)
            this.__math = new MathService();
        return this.__math;
    }
    get array() {
        if (this.__array === null)
            this.__array = new ArrayService();
        return this.__array;
    }
    get dictionary() {
        if (this.__dictionary === null)
            this.__dictionary = new DictionaryManipulatorJS();
        return this.__dictionary;
    }
    get text() {
        if (this.__text === null)
            this.__text = new TextManipulationService();
        return this.__text;
    }
    /**
     * Execute function on document ready
     * @param {Function} _func
     */
    ready(_func) {
        $(document).ready(_func);
    }

    /**
     * Use this method to extend core with additinonal functionality.
     * @param {any} module
     */
    extend(module, options) {
        $.extend(this, module);
    }
}

class ObjectManipulationJS {
    constructor() {

    }
    /**
    * get jquery from string query or DOMElement
    * @param {string | DOMElement} query
    */
    getJQuery(query) {
        if (query && (typeof query === 'string' || this.isDOMElement(query)))
            return $(query);
        else return null;
    }
    /**
     * get jquery element by id. Query is not required to have # appended
     * @param {string} query
     */
    getJQueryById(query) {
        let id = this.getQueryId(query);
        if (id) return $(id);
        return null;
    }

    /**
     * convert string to jquery id target
     * @param {any} query
     */
    getQueryId(query) {
        if (query && typeof query === 'string') {
            if (query.startsWith('#')) return query;
            else return '#' + query;
        }
        return null;
    }
    /**
    * Search for member in object. If found return default value, else return default value (null if not provided)
    * @param {Object} obj
    * @param {string} member
    * @param {any} _defaultValue
    */
    get(obj, member, _defaultValue) {
        if (!(obj)) return _defaultValue;

        if (!_defaultValue)
            _defaultValue = null;

        if (member in obj) {
            return obj[member];
        }
        else return _defaultValue;
    }
    /**
    * Search for member in object. If found check if it is function. Default value: null
    * @param {Object} obj
    * @param {string} member
    */
    getFunction(obj, member) {
        let value = this.get(obj, member, null);

        if (value && this.isFunction(value)) return value;
        else return null;
    }
    mockFunction(func) {
        if (!this.isFunction(func)) return () => { };
        else return func;
    }
    htmlStringify(dom) {
        if (this.isDOM(dom)) {
            let parent = document.createElement('DIV');
            parent.appendChild(dom);
            return parent.innerHTML;
        }
        else return "";
    }

    /**
     * hide DOM element
     * @param {Element} element
     */
    hide(element) {
        element.classList.add("d-none");
    }

    /**
     * Execute function on first click on element
     * @param {Object} options
     */
    onClickLoad(options) {
        options['event'] = 'click';
        this.executeOnFirstTrigger(options);
    }

    /**
     * execute callback on first trigger of the event
     * @param {any} options
     */
    executeOnFirstTrigger(options) {
        var elementID = _core.object.get(options, "element", null);
        var callback = _core.object.get(options, "callback", null);
        var block = _core.object.get(options, "block", null);
        var release = _core.object.get(options, "release", null);
        var event = _core.object.get(options, "event", 'click');
        if (elementID !== null && elementID !== undefined && callback !== null && _core.object.isFunction(callback)) {

            var element = document.getElementById(elementID);

            if (element !== null && element !== undefined) {
                $(element).on(event, function () {
                    if (element.hasAttribute("data-on-click-executed")) {
                        return;
                    }
                    else {
                        var title = _core.object.get(options, "title", "");
                        var description = _core.object.get(options, "description", "");
                        element.setAttribute("data-on-click-executed", "true");
                        if (!block)
                            _core.swall.startLoader(title, description);
                        else
                            block();


                        callback(function () {
                            if (!release)
                                _core.swall.closeLoader();
                            else
                                release();
                        });

                    }
                });
            }
        }
    }

    /**
     * check if variable contains a function
     * @param {Function} functionToCheck
     */
    isFunction(functionToCheck) {
        return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
    }

    /**
     * extract member from object. If value is function type, it will be retuned,otherwise null
     * @param {any} obj
     * @param {any} member
     */
    getFunction(obj, member) {
        var callback = this.get(obj, member, null);

        if (this.isFunction(callback)) return callback;
        else return null;
    }
    /**
     * extract anti forgery token from form and append it to object
     * @param {any} atiForgeryFormID
     * @param {any} object
     */
    appendAntiForgeryToken(atiForgeryFormID, object) {
        var form = document.getElementById(atiForgeryFormID);
        if (form !== null && form !== undefined) {
            var token = form.getElementsByTagName('input');
            if (token.length > 0) {
                for (var i = 0; i < token.length; i++) {
                    if (token[i].name === '__RequestVerificationToken') {
                        object['__RequestVerificationToken'] = token[i].value;
                    }
                }

            }
        }
        return object;
    }

    /**
     * add classes to elemt
     * @param {any} element
     * @param {any} className
     */
    addClass(element, className) {
        if (!element || typeof className === 'undefined') {
            return;
        }

        var classNames = className.split(' ');

        if (element.classList) {
            for (var i = 0; i < classNames.length; i++) {
                if (classNames[i] && classNames[i].length > 0) {
                    element.classList.add(classNames[i].trim());
                }
            }
        }
    }

    /**
    * add classes to elemt
    * @param {any} element
    * @param {any} className
    */
    removeClass(element, className) {
        if (!element || typeof className === 'undefined') {
            return;
        }

        var classNames = className.split(' ');

        if (element.classList) {
            for (var i = 0; i < classNames.length; i++) {
                if (classNames[i] && classNames[i].length > 0) {
                    element.classList.remove(classNames[i].trim());
                }
            }
        }
    }

    actualCss(el, prop, cache) {
        var css = '';

        if (el instanceof HTMLElement === false) {
            return;
        }

        if (!el.getAttribute('kt-hidden-' + prop) || cache === false) {
            var value;

            // the element is hidden so:
            // making the el block so we can meassure its height but still be hidden
            css = el.style.cssText;
            el.style.cssText = 'position: absolute; visibility: hidden; display: block;';

            if (prop == 'width') {
                value = el.offsetWidth;
            } else if (prop == 'height') {
                value = el.offsetHeight;
            }

            el.style.cssText = css;

            // store it in cache
            el.setAttribute('kt-hidden-' + prop, value);

            return parseFloat(value);
        } else {
            // store it in cache
            return parseFloat(el.getAttribute('kt-hidden-' + prop));
        }
    }

    /**
     * remove DOM element
     * @param {any} element
     */
    removeDOM(element) {
        if (element && element.parentNode) {
            element.parentNode.removeChild(element);
        }
    }

    /**
     * convert js array to js dictionary
     * @param {any} array
     * @param {any} key
     */
    toDictionary(array, key) {
        var dic = [];

        for (var i = 0; i < array.length; i++) {
            dic[array[i][key]] = array[i];
        }

        return dic;
    }

    /**
     * check if object is DOM element
     * @param {any} object
     */
    isDOMElement(object) {
        try {
            //Using W3 DOM2 (works for FF, Opera and Chrome)
            return object instanceof HTMLElement;
        }
        catch (e) {
            //Browsers not supporting W3 DOM2 don't have HTMLElement and
            //an exception is thrown and we end up here. Testing some
            //properties that all elements have (works on IE7)
            return (typeof object === "object") && (object.nodeType === 1) && (typeof object.style === "object") && (typeof object.ownerDocument === "object");
        }
    }
    isDOM(object) { return this.isDOMElement(object); }
    /**
     * check if object is DOM element
     * @param {any} object
     */
    isSVGElement(object) {
        try {
            //Using W3 DOM2 (works for FF, Opera and Chrome)
            return object instanceof SVGElement;
        }
        catch (e) {
            //Browsers not supporting W3 DOM2 don't have HTMLElement and
            //an exception is thrown and we end up here. Testing some
            //properties that all elements have (works on IE7)
            return (typeof object === "object") && (object.nodeType === 1) && (typeof object.style === "object") && (typeof object.ownerDocument === "object");
        }
    }

    dispatchEvent(target, event, options) {
        if (this.isDOM(target) && !_core.text.isNullOrEmpty(event)) {
            let devent = new CustomEvent(event, { bubbles: false, detail: options });
            target.dispatchEvent(devent);

            let jevent = jQuery.Event(event, { detail: options });
            $(target).trigger(jevent);
        }
    }
    clone(object) {
        return JSON.parse(JSON.stringify(object))
    }
    /**
     * Promise version for wait function.
     * @param {any} miliseconds
     */
    async waitAsync(miliseconds = 1000) {
        return new Promise((resolve, reject) => {
            setTimeout(() => { resolve() }, miliseconds);
        });
    }
}

class ConverterService {
    constructor() {

    }
    /**
     * try to convert to text to bool
     * @param {string} toConvert
     */
    safeBoolParse(toConvert) {

        if (toConvert === 1 || toConvert == '1' || toConvert == 'true' || toConvert == "True" || toConvert === true)
            return true;
        else
            return false;

    }
    /**
     * convert provided text to integer. If conversion is not possible, default value will be returned
     * @param {string} toConvert 
     * @param {int} _default if is not an integer, o will be used as default instead
     */
    safeIntParse(toConvert, _default, forceNull = false) {
        if (!Number.isInteger(_default)) {
            if (forceNull) _default = null;
            else _default = 0;
        }

        if (isNaN(toConvert)) return _default;

        var result = parseInt(toConvert);
        if (isNaN(result))
            return _default
        else
            return result;

    }
}

class TextManipulationService {
    constructor() {

    }
    #escapeRegExp(string) {
        return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
    }
    replaceAll(str, find, replace) {
        return str.replace(new RegExp(this.#escapeRegExp(find), 'g'), replace);
    }
    isNullOrEmpty(text) {
        if (!text || typeof text !== "string" || text === '') return true;
        return false;
    }
    isNullOrWhiteSpace(text) {
        if (!text || typeof text !== "string" || text === '' || text === ' ' || text.trim().length === 0) return true;
        return false;
    }
}

class AjaxService {
    constructor() {

    }
    #getGlobal() {
        if (document.globals && document.globals.ajax && name in document.globals.ajax && context.object.isFunction(document.globals.ajax[name])) return document.globals.ajax[name];
        return null;
    };
    #getErrorModel(jqXHR) {
        if (jqXHR) {
            if (jqXHR.responseJSON)
                return jqXHR.responseJSON;
            if (jqXHR.responseText)
                return jqXHR.responseText;
        }
        return null;
    };

    #createBody(url, data, options) {
        let self = this;

        const unauthorizedBody = (data, jqXHR) => {
            let parameters = _core.object.get(options, 'parameters', null);

            let callback = _core.object.get(options, 'unauthorized', null) ?? _core.object.get(options, 'error', null) ?? this.#getGlobal("unauthorized");
            if (callback) {
                callback(self.#getErrorModel(jqXHR), jqXHR);
                return;
            }
        }

        const forbiddenBody = (data, jqXHR) => {
            let parameters = _core.object.get(options, 'parameters', null);

            let callback = _core.object.get(options, 'forbidden', null) ?? _core.object.get(options, 'error', null) ?? this.#getGlobal("forbidden");
            if (callback) {
                callback(self.#getErrorModel(jqXHR), jqXHR);
                return;
            }
        }

        const defaultErrorBody = (data, jQXHR) => {
            let parameters = _core.object.get(options, 'parameters', null);

            let callback = _core.object.get(options, 'error', null) ?? this.#getGlobal("error");
            if (callback) {
                callback(data, jQXHR);
                return;
            }
        }

        return {
            type: _core.object.get(options, 'method', "POST"),
            url: url,
            async: _core.converter.safeBoolParse(_core.object.get(options, 'async', true)),
            data: data,
            contentType: _core.object.get(options, 'contentType', 'application/x-www-form-urlencoded; charset=UTF-8'),
            cache: _core.converter.safeBoolParse(_core.object.get(options, 'cache', false)),
            headers: _core.object.get(options, 'headers', null),
            dataType: _core.object.get(options, 'dataType', null),
            processData: _core.converter.safeBoolParse(_core.object.get(options, 'processData', true)),
            timeout: _core.converter.safeIntParse(_core.object.get(options, 'timeout', 60000)),
            success: function (data, statusCode, jqXHR) {
                let callback = _core.object.get(options, 'success', null);
                let parameters = _core.object.get(options, 'parameters', null);
                if (callback !== null) {
                    callback(data, parameters, statusCode, jqXHR);
                }
            },
            error: function (jqXHR, statusCode) {
                let data = self.#getErrorModel(jqXHR);

                switch (jqXHR.status) {
                    case 401: {
                        unauthorizedBody(data, jqXHR);
                        break;
                    }
                    case 403: {
                        forbiddenBody(data, jqXHR);
                        break;
                    }
                    default: {
                        defaultErrorBody(data, jqXHR);
                        break;
                    }
                }

            },
            complete: function (jqXHR, statusCode) {
                let callback = _core.object.get(options, 'complete', null);
                let parameters = _core.object.get(options, 'parameters', null);
                if (callback !== null)
                    callback(parameters, statusCode, jqXHR);
            },
        };
    }

    request(url, data, options) {
        if (typeof options["promise"] === "boolean" && options["promise"]) {
            return new Promise((accept, reject) => {
                if (!options)
                    options = {};
                options["success"] = function (_data, statusCode, jqXHR) {
                    accept({ data: _data, statusCode, jqXHR });
                };
                options["error"] = function (data, jqXHR) {
                    reject({ data, jqXHR });
                }
                let body = this.#createBody(url, data, options);
                $.ajax(body);
            });
        }
        else {
            $.ajax(this.#createBody(url, data, options));
        }
    }
    /**
     * is response valid
     * @param {any} data
     */
    isSuccessful(data) {
        if (data.hasOwnProperty("code")) {
            switch (data.code) {
                case 0: return true;
                default: return false;
            }
        }
        else
            return false;
    }

    generalFailure(display = 0, refresh = false) {
        swal.fire({
            title: "Oooops !",
            text: "An error was encountered while processing your request. If this problem persists please contact us !",
            confirmButtonText: "OK, I Understand",
            icon: 'error'
        }).then(function () {
            if (refresh) {
                window.location.reload();
            }
        });
    }

    displayLogs(logs) {
        for (var i = 0; i < logs.length; i++) {
            switch (logs[i].level) {
                case 0:
                    {
                        toastr.success(logs[i].message);
                        break;
                    }
                case 1:
                    {
                        toastr.info(logs[i].message);
                        break;
                    }
                case 2:
                    {
                        toastr.warning(logs[i].message);
                        break;
                    }
                case 3:
                    {
                        toastr.error(logs[i].message);
                        break;
                    }
                default: break;
            }
        }
    }
}

class SwallService {
    constructor() {
        this.loaderHandler = null;
        this.loaderStartDate = null;
    }
    /**
     * start swall loader
     * @param {string} title
     * @param {string} text
     * @param {boolean} log start loader in logging mode
     */
    startLoader(title = "", text = "", log = false) {
        this.loaderStartDate = new Date();

        var html = "";
        var timer = 6500000;
        if (log) {
            html = '<div class="w-100 d-flex flex-column justify-content-center" id="__swalLogger"></div>';
            timer = 6500000;
        }

        if (log) {
            this.loaderHandler = swal.fire({
                title: title,
                text: text,
                html: html,
                closeOnClickOutside: false,
                allowOutsideClick: false,
                onOpen: function () {
                    swal.showLoading()
                }
            });
        }
        else {
            this.loaderHandler = swal.fire({
                title: title,
                text: text,
                html: html,
                timer: timer,
                closeOnClickOutside: false,
                allowOutsideClick: false,
                onOpen: function () {
                    swal.showLoading()
                }
            });
        }
    }
    /**
     * add new log to loader
     * @param {any} content
     */
    addLog(content, override = false) {
        var container = document.getElementById("__swalLogger");
        if (container != null && container !== undefined) {
            if (override) {
                container.innerHTML = '<div class="w-100">' + content + '</div>';
            }
            else {
                container.innerHTML += '<div class="w-100">' + content + '</div>';
            }
        }
    }
    /**
     * close swall loader
     * @param {any} imediate close it now or wait 500 ms to close it
     */
    closeLoader(imediate = false) {
        try {
            if (imediate) {
                this.loaderHandler.close();
            }
            else {
                var currentDate = new Date();
                if ((currentDate.getTime() - this.loaderStartDate.getTime()) < 700) {
                    setTimeout(function () {
                        _core.swall.closeLoader(true);
                    }, 500);
                }
                else {
                    this.loaderHandler.close();
                }
            }
        }
        catch (err) {
            window.location.href(true);
        }
    }

    /**
     * chreate swall with message, which refresh page after confirm
     * @param {any} title
     * @param {any} message
     * @param {any} outcome
     */
    refreshAlert(title, message = 'Page will refresh after confirmation', outcome = 'success', button = 'OK') {
        swal.fire({
            title: title,
            text: message,
            closeOnClickOutside: false,
            allowOutsideClick: false,
            confirmButtonText: button,
            icon: outcome
        }).then(function () { window.location.reload(true); });
    }
    /**
     * Display simple swall
     * @param {any} title
     * @param {any} message
     * @param {any} button
     * @param {any} outcome
     */
    display(title, message = '', button = 'OK', outcome = 'success') {
        swal.fire({
            title: title,
            text: message,
            confirmButtonText: button,
            icon: outcome
        });
    }

    displayConfirmation(title, message = '', confirmationButton = 'YES', rejectButton = 'NO', onConfirmation, onReject, allowOutsideClick = true) {
        swal.fire({
            icon: 'warning',
            title: title,
            text: message,
            buttonsStyling: false,
            showCancelButton: true,
            showCancelButton: true,
            confirmButtonText: confirmationButton,
            cancelButtonText: rejectButton,
            allowOutsideClick: _core.converter.safeBoolParse(allowOutsideClick, true),
            allowEscapeKey: _core.converter.safeBoolParse(allowOutsideClick, true), 
            customClass: {
                confirmButton: "btn btn-primary",
                denyButton: 'btn btn-danger',
                cancelButton: 'btn btn-warning',
            }
        }).then((result) => {
            if (result.isConfirmed)
                onConfirmation();
            else
                onReject();
        });
    }
}

class ValidatorService {
    constructor() {

    }
    /**
     * validate form
     * jquery - if jquery is true, use jquery.validate.js else formvalidation.io
     * html - use html atributes to pass constrains and messages required for validation
     * constrains - rules for validate
     * form - target DOM form
     * callback - function to be called
     * @param {any} options
     */
    validate(options) {
        //if jquery is true, use jquery.validate.js else formvalidation.io
        var jquery = _core.converter.safeBoolParse(_core.object.get(options, "jquery", false));
        //use constrains to validate
        var constrains = null;
        //use attributes to validate
        var html = _core.converter.safeBoolParse(_core.object.get(options, "html", false));
        //target DOM form
        var form = _core.object.get(options, "form", null);

        if (form !== null && form !== undefined) {
            var response = {
                status: false
            }
            if (jquery) {
                response.status = form.valid();
            }
            else {

            }
            return response;
        }

        return null;

    }
}

class LoaderService {
    constructor() {
        this.__fullPanel = null;
    }

    getFullPagePanel() {
        if (this.__fullPanel == null) {
            this.__fullPanel = {
                DOM: null,
                instance: function () {


                    if (this.DOM == null) {

                        this.DOM = document.getElementById("__full--panel-ldr");

                        if (this.DOM == null) {
                            this.DOM = document.createElement("DIV");

                            this.DOM.classList.add("full-page-loader-panel");
                            this.DOM.style = "display:none";
                            this.DOM.id = "__full--panel-ldr";
                            document.body.appendChild(this.DOM);
                        }

                    }
                    return this.DOM;
                },
                show: function (html) {
                    this.instance().classList.add("show");
                    this.instance().style = "display:flex";
                    this.instance().innerHTML = html;
                },
                hide: function () {
                    this.instance().classList.remove("show");
                    this.instance().style = "display:none";
                    this.instance().innerHTML = "";
                }
            }
        }
        return this.__fullPanel;
    }

    displayFullLoader() {
        var panel = this.getFullPagePanel();

        panel.show('<div><div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div></div>');

    }

    hideFullLoader() {
        var panel = this.getFullPagePanel();
        panel.hide();
    }
    block(target, options) {
        var el = $(target);

        options = $.extend(true, {
            opacity: 0.05,
            overlayColor: '#000000',
            type: '',
            size: '',
            state: 'primary',
            centerX: true,
            centerY: true,
            message: '',
            shadow: true,
            width: 'auto'
        }, options);

        var html;
        var version = options.type ? 'spinner-' + options.type : 'spinner-border';
        var state = options.state ? 'text-' + options.state : '';
        var size = options.size ? 'spinner-' + options.size : '';
        var spinner = '<div class="spinner ' + version + ' ' + state + ' ' + size + '"></div>';

        if (options.message && options.message.length > 0) {
            var classes = 'blockui ' + (options.shadow === false ? 'blockui' : '');

            html = '<div class="' + classes + '"><span>' + options.message + '</span>' + spinner + '</div>';

            var el = document.createElement('div');

            $('body').prepend(el);
            _core.object.addClass(el, classes);
            el.innerHTML = html;
            options.width = _core.object.actualCss(el, 'width') + 10;
            _core.object.removeDOM(el);

            if (target == 'body') {
                html = '<div class="' + classes + '" style="margin-left:-' + (options.width / 2) + 'px;"><span>' + options.message + '</span><span>' + spinner + '</span></div>';
            }
        } else {
            html = spinner;
        }

        var params = {
            message: html,
            centerY: options.centerY,
            centerX: options.centerX,
            css: {
                top: '30%',
                left: '50%',
                border: '0',
                padding: '0',
                backgroundColor: 'none',
                width: options.width
            },
            overlayCSS: {
                backgroundColor: options.overlayColor,
                opacity: options.opacity,
                cursor: 'wait',
                zIndex: (target == 'body' ? 1100 : 10)
            },
            onUnblock: function () {
                if (el && el[0]) {
                    $(el[0]).prop('position', '');
                    $(el[0]).prop('zoom', '');
                }
            }
        };

        if (target == 'body') {
            params.css.top = '50%';
            $.blockUI(params);
        } else {
            var el = $(target);
            el.block(params);
        }
    }

    release(target) {
        if (target && target != 'body') {
            $(target).unblock();
        } else {
            $.unblockUI();
        }
    }
}

class MathService {
    constructor() {

    }
    /**
     * Get random number between min and max
     * @param {any} min
     * @param {any} max
     */
    random(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }
    isNumber(value) {
        return (Math.floor(value) == value && $.isNumeric(value));
    }
}

class ArrayService {
    constructor() {
    }

    /**
     * remove element from array by value
     * @param {any} array
     * @param {any} value
     */
    removeByValue(array, value) {
        for (var i = 0; i < array.length; i++) {

            if (array[i] === value) {

                array.splice(i, 1);
                return;
            }

        }
    }
    /**
      * remove element from array at index
      * @param {Array} array
      * @param {Int} index
      */
    removeAt() {
        if (index > -1) {
            array.splice(index, 1);
        }
    }
    /**
    * remove element from array witch certain value
    * @param {any} arr
    * @param {any} val
    */
    removeValue(arr, val) {
        var poz = arr.indexOf(val);
        if (poz > -1)
            arr.splice(poz, 1);
    }
    /**
     * deep copy array
     * @param {any} array
     */
    copy(array) {
        var cpy = [];
        for (var i = 0; i < array.length; i++)
            cpy.push(array[i]);

        return cpy;
    }
    /**
     * intersect 2 arrays
     * @param {any} array1
     * @param {any} array2
     */
    intersect(array1, array2) {
        return array1.filter(value => array2.includes(value));
    }
    /**
     * exclude from array1 array2
     * @param {any} array1
     * @param {any} array2
     */
    exclude(array1, array2) {
        return array1.filter(x => !array2.includes(x));
    }
    /**
     * extract member from object and add it to array
     * @param {any} array
     * @param {any} member
     */
    selectMember(array, member) {
        var cpy = [];
        for (var i = 0; i < array.length; i++) {
            cpy.push(array[i][member]);
        }
        return cpy;
    }
    select(array, callback) {
        var cpy = [];
        for (var i = 0; i < array.length; i++) {
            cpy.push(callback(array[i]));
        }
        return cpy;
    }
    containsWM(array, member, value) {
        var found = false;
        for (var i = 0; i < array.length; i++) {
            if (array[i][member] === value) {
                found = true;
                break;
            }
        }
        return found;
    }
    contains(array, value) {
        return array.includes(value);
    }
}

class DictionaryManipulatorJS {
    constructor() {
    }

    toDictionary(array, key) {

        var cpy = [];
        for (var i = 0; i < array.length; i++) {
            cpy[array[i][key]] = array[i];
        }
        return cpy;

    }
    values(dictionary) {
        var keys = Object.keys(dictionary);

        var cpy = [];

        for (var i = 0; i < keys.length; i++)
            cpy.push(dictionary[keys[i]]);

        return cpy;


    }
    keys(dictionary) {
        return Object.keys(dictionary);
    }
}

const _core = new CoreJS();