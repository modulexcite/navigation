﻿class Segment {
    path: string;
    optional: boolean;
    defaults: any;
    pattern: string = '';
    params: string[] = [];
    private subSegments: { name: string; param: boolean }[] = [];
    private subSegmentPattern: RegExp = /[{]{0,1}[^{}]+[}]{0,1}/g;
    private escapePattern: RegExp = /[\.+*\^$\[\](){}']/g;

    constructor(path: string, optional: boolean, defaults?: any) {
        this.path = path;
        this.optional = optional;
        this.defaults = defaults;
        this.parse();
    }

    private parse() {
        if (this.path.length === 0)
            return;
        var matches = this.path.match(this.subSegmentPattern);
        for (var i = 0; i < matches.length; i++) {
            var subSegment = matches[i];
            if (subSegment.charAt(0) == '{') {
                var param = subSegment.substring(1, subSegment.length - 1);
                var name = param.slice(-1) === '?' ? param.slice(0, -1) : param;
                this.params.push(name);
                this.subSegments.push({ name: name, param: true });
                var optionalOrDefault = param.slice(-1) === '?' || this.defaults[name];
                this.optional = this.optional && this.path.length === subSegment.length && optionalOrDefault;
                this.pattern += !this.optional ? '([^/]+)' : '(\/[^/]+)?';
            } else {
                this.optional = false;
                this.subSegments.push({ name: subSegment, param: false });
                this.pattern += subSegment.replace(this.escapePattern, '\\$&');
            }
        }
        if (!this.optional)
            this.pattern = '\/' + this.pattern;
    }

    build(data?: any): { path: string; optional: boolean } {
        var routePath = '';
        var optional = this.optional;
        var blank = false;
        for(var i = 0; i < this.subSegments.length; i++) {
            var subSegment = this.subSegments[i];
            if (!subSegment.param) {
                routePath += subSegment.name;
            } else {
                var val = data[subSegment.name];
                var defaultVal = this.defaults[subSegment.name];
                optional = optional && (!val || val === defaultVal);
                val = val ? val : defaultVal;
                blank = blank || !val;
                routePath += encodeURIComponent(val);
            }
        }
        return { path: !blank ? routePath : null, optional: optional };
    }
}
export = Segment;
