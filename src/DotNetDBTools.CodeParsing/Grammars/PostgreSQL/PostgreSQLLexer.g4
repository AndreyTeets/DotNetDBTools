/*
 This file includes work originally covered by the following copyright and permission notices:

 Licensed to the Apache Software Foundation (ASF) under one
 or more contributor license agreements.  See the NOTICE file
 distributed with this work for additional information
 regarding copyright ownership.  The ASF licenses this file
 to you under the Apache License, Version 2.0 (the
 "License"); you may not use this file except in compliance
 with the License.  You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

lexer grammar PostgreSQLLexer;

options {
    language=CSharp;
}
@header {
using System.Collections.Generic;
using System.Diagnostics;
}

@members {
/* This field stores the tags which are used to detect the end of a dollar-quoted string literal.
*/
private readonly Stack<String> _tags = new Stack<String>();
}

    /*
     * Most keyword tokens are autogenerated using the list in the Keyword class,
     * which comes directly from PostgreSQL source.
     * Each word-token must also participate in a parser rule corresponding to
     * its keyword's category. These are autogenerated using Keyword class.
     *
     * Manually added word-tokens must also be manually added to the
     * tokens_nonkeyword parser rule.
     */

    /*
    ==================================================
    UNRESERVED_KEYWORD
    ==================================================
    */

    ABORT: [aA] [bB] [oO] [rR] [tT];    // first identifier rule, sync with CustomSQLAntlrErrorStrategy & AntlrUtils
    ABSOLUTE: [aA] [bB] [sS] [oO] [lL] [uU] [tT] [eE];
    ACCESS: [aA] [cC] [cC] [eE] [sS] [sS];
    ACTION: [aA] [cC] [tT] [iI] [oO] [nN];
    ADD: [aA] [dD] [dD];
    ADMIN: [aA] [dD] [mM] [iI] [nN];
    AFTER: [aA] [fF] [tT] [eE] [rR];
    AGGREGATE: [aA] [gG] [gG] [rR] [eE] [gG] [aA] [tT] [eE];
    ALSO: [aA] [lL] [sS] [oO];
    ALTER: [aA] [lL] [tT] [eE] [rR];
    ALWAYS: [aA] [lL] [wW] [aA] [yY] [sS];
    ASENSITIVE: [aA] [sS] [eE] [nN] [sS] [iI] [tT] [iI] [vV] [eE];
    ASSERTION: [aA] [sS] [sS] [eE] [rR] [tT] [iI] [oO] [nN];
    ASSIGNMENT: [aA] [sS] [sS] [iI] [gG] [nN] [mM] [eE] [nN] [tT];
    AT: [aA] [tT];
    ATOMIC: [aA] [tT] [oO] [mM] [iI] [cC];
    ATTACH: [aA] [tT] [tT] [aA] [cC] [hH];
    ATTRIBUTE: [aA] [tT] [tT] [rR] [iI] [bB] [uU] [tT] [eE];

    BACKWARD: [bB] [aA] [cC] [kK] [wW] [aA] [rR] [dD];
    BEFORE: [bB] [eE] [fF] [oO] [rR] [eE];
    BEGIN: [bB] [eE] [gG] [iI] [nN];
    BREADTH: [bB] [rR] [eE] [aA] [dD] [tT] [hH];
    BY: [bB] [yY];

    CACHE: [cC] [aA] [cC] [hH] [eE];
    CALL: [cC] [aA] [lL] [lL];
    CALLED: [cC] [aA] [lL] [lL] [eE] [dD];
    CASCADE: [cC] [aA] [sS] [cC] [aA] [dD] [eE];
    CASCADED: [cC] [aA] [sS] [cC] [aA] [dD] [eE] [dD];
    CATALOG: [cC] [aA] [tT] [aA] [lL] [oO] [gG];
    CHAIN: [cC] [hH] [aA] [iI] [nN];
    CHARACTERISTICS: [cC] [hH] [aA] [rR] [aA] [cC] [tT] [eE] [rR] [iI] [sS] [tT] [iI] [cC] [sS];
    CHECKPOINT: [cC] [hH] [eE] [cC] [kK] [pP] [oO] [iI] [nN] [tT];
    CLASS: [cC] [lL] [aA] [sS] [sS];
    CLOSE: [cC] [lL] [oO] [sS] [eE];
    CLUSTER: [cC] [lL] [uU] [sS] [tT] [eE] [rR];
    COLUMNS: [cC] [oO] [lL] [uU] [mM] [nN] [sS];
    COMMENT: [cC] [oO] [mM] [mM] [eE] [nN] [tT];
    COMMENTS: [cC] [oO] [mM] [mM] [eE] [nN] [tT] [sS];
    COMMIT: [cC] [oO] [mM] [mM] [iI] [tT];
    COMMITTED: [cC] [oO] [mM] [mM] [iI] [tT] [tT] [eE] [dD];
    COMPRESSION: [cC] [oO] [mM] [pP] [rR] [eE] [sS] [sS] [iI] [oO] [nN];
    CONFIGURATION: [cC] [oO] [nN] [fF] [iI] [gG] [uU] [rR] [aA] [tT] [iI] [oO] [nN];
    CONFLICT: [cC] [oO] [nN] [fF] [lL] [iI] [cC] [tT];
    CONNECTION: [cC] [oO] [nN] [nN] [eE] [cC] [tT] [iI] [oO] [nN];
    CONSTRAINTS: [cC] [oO] [nN] [sS] [tT] [rR] [aA] [iI] [nN] [tT] [sS];
    CONTENT: [cC] [oO] [nN] [tT] [eE] [nN] [tT];
    CONTINUE: [cC] [oO] [nN] [tT] [iI] [nN] [uU] [eE];
    CONVERSION: [cC] [oO] [nN] [vV] [eE] [rR] [sS] [iI] [oO] [nN];
    COPY: [cC] [oO] [pP] [yY];
    COST: [cC] [oO] [sS] [tT];
    CSV: [cC] [sS] [vV];
    CUBE: [cC] [uU] [bB] [eE];
    CURRENT: [cC] [uU] [rR] [rR] [eE] [nN] [tT];
    CURSOR: [cC] [uU] [rR] [sS] [oO] [rR];
    CYCLE: [cC] [yY] [cC] [lL] [eE];

    DATA: [dD] [aA] [tT] [aA];
    DATABASE: [dD] [aA] [tT] [aA] [bB] [aA] [sS] [eE];
    DAY: [dD] [aA] [yY];
    DEALLOCATE: [dD] [eE] [aA] [lL] [lL] [oO] [cC] [aA] [tT] [eE];
    DECLARE: [dD] [eE] [cC] [lL] [aA] [rR] [eE];
    DEFAULTS: [dD] [eE] [fF] [aA] [uU] [lL] [tT] [sS];
    DEFERRED: [dD] [eE] [fF] [eE] [rR] [rR] [eE] [dD];
    DEFINER: [dD] [eE] [fF] [iI] [nN] [eE] [rR];
    DELETE: [dD] [eE] [lL] [eE] [tT] [eE];
    DELIMITER: [dD] [eE] [lL] [iI] [mM] [iI] [tT] [eE] [rR];
    DELIMITERS: [dD] [eE] [lL] [iI] [mM] [iI] [tT] [eE] [rR] [sS];
    DEPENDS: [dD] [eE] [pP] [eE] [nN] [dD] [sS];
    DEPTH: [dD] [eE] [pP] [tT] [hH];
    DETACH: [dD] [eE] [tT] [aA] [cC] [hH];
    DICTIONARY: [dD] [iI] [cC] [tT] [iI] [oO] [nN] [aA] [rR] [yY];
    DISABLE: [dD] [iI] [sS] [aA] [bB] [lL] [eE];
    DISCARD: [dD] [iI] [sS] [cC] [aA] [rR] [dD];
    DOCUMENT: [dD] [oO] [cC] [uU] [mM] [eE] [nN] [tT];
    DOMAIN: [dD] [oO] [mM] [aA] [iI] [nN];
    DOUBLE: [dD] [oO] [uU] [bB] [lL] [eE];
    DROP: [dD] [rR] [oO] [pP];

    EACH: [eE] [aA] [cC] [hH];
    ENABLE: [eE] [nN] [aA] [bB] [lL] [eE];
    ENCODING: [eE] [nN] [cC] [oO] [dD] [iI] [nN] [gG];
    ENCRYPTED: [eE] [nN] [cC] [rR] [yY] [pP] [tT] [eE] [dD];
    ENUM: [eE] [nN] [uU] [mM];
    ESCAPE: [eE] [sS] [cC] [aA] [pP] [eE];
    EVENT: [eE] [vV] [eE] [nN] [tT];
    EXCLUDE: [eE] [xX] [cC] [lL] [uU] [dD] [eE];
    EXCLUDING: [eE] [xX] [cC] [lL] [uU] [dD] [iI] [nN] [gG];
    EXCLUSIVE: [eE] [xX] [cC] [lL] [uU] [sS] [iI] [vV] [eE];
    EXECUTE: [eE] [xX] [eE] [cC] [uU] [tT] [eE];
    EXPLAIN: [eE] [xX] [pP] [lL] [aA] [iI] [nN];
    EXPRESSION: [eE] [xX] [pP] [rR] [eE] [sS] [sS] [iI] [oO] [nN];
    EXTENSION: [eE] [xX] [tT] [eE] [nN] [sS] [iI] [oO] [nN];
    EXTERNAL: [eE] [xX] [tT] [eE] [rR] [nN] [aA] [lL];

    FAMILY: [fF] [aA] [mM] [iI] [lL] [yY];
    FILTER: [fF] [iI] [lL] [tT] [eE] [rR];
    FINALIZE: [fF] [iI] [nN] [aA] [lL] [iI] [zZ] [eE];
    FIRST: [fF] [iI] [rR] [sS] [tT];
    FOLLOWING: [fF] [oO] [lL] [lL] [oO] [wW] [iI] [nN] [gG];
    FORCE: [fF] [oO] [rR] [cC] [eE];
    FORWARD: [fF] [oO] [rR] [wW] [aA] [rR] [dD];
    FUNCTION: [fF] [uU] [nN] [cC] [tT] [iI] [oO] [nN];
    FUNCTIONS: [fF] [uU] [nN] [cC] [tT] [iI] [oO] [nN] [sS];

    GENERATED: [gG] [eE] [nN] [eE] [rR] [aA] [tT] [eE] [dD];
    GLOBAL: [gG] [lL] [oO] [bB] [aA] [lL];
    GRANTED: [gG] [rR] [aA] [nN] [tT] [eE] [dD];
    GROUPS: [gG] [rR] [oO] [uU] [pP] [sS];

    HANDLER: [hH] [aA] [nN] [dD] [lL] [eE] [rR];
    HEADER: [hH] [eE] [aA] [dD] [eE] [rR];
    HOLD: [hH] [oO] [lL] [dD];
    HOUR: [hH] [oO] [uU] [rR];

    IDENTITY: [iI] [dD] [eE] [nN] [tT] [iI] [tT] [yY];
    IF: [iI] [fF];
    IMMEDIATE: [iI] [mM] [mM] [eE] [dD] [iI] [aA] [tT] [eE];
    IMMUTABLE: [iI] [mM] [mM] [uU] [tT] [aA] [bB] [lL] [eE];
    IMPLICIT: [iI] [mM] [pP] [lL] [iI] [cC] [iI] [tT];
    IMPORT: [iI] [mM] [pP] [oO] [rR] [tT];
    INCLUDE: [iI] [nN] [cC] [lL] [uU] [dD] [eE];
    INCLUDING: [iI] [nN] [cC] [lL] [uU] [dD] [iI] [nN] [gG];
    INCREMENT: [iI] [nN] [cC] [rR] [eE] [mM] [eE] [nN] [tT];
    INDEX: [iI] [nN] [dD] [eE] [xX];
    INDEXES: [iI] [nN] [dD] [eE] [xX] [eE] [sS];
    INHERIT: [iI] [nN] [hH] [eE] [rR] [iI] [tT];
    INHERITS: [iI] [nN] [hH] [eE] [rR] [iI] [tT] [sS];
    INLINE: [iI] [nN] [lL] [iI] [nN] [eE];
    INPUT: [iI] [nN] [pP] [uU] [tT];
    INSENSITIVE: [iI] [nN] [sS] [eE] [nN] [sS] [iI] [tT] [iI] [vV] [eE];
    INSERT: [iI] [nN] [sS] [eE] [rR] [tT];
    INSTEAD: [iI] [nN] [sS] [tT] [eE] [aA] [dD];
    INVOKER: [iI] [nN] [vV] [oO] [kK] [eE] [rR];
    ISOLATION: [iI] [sS] [oO] [lL] [aA] [tT] [iI] [oO] [nN];

    KEY: [kK] [eE] [yY];

    LABEL: [lL] [aA] [bB] [eE] [lL];
    LANGUAGE: [lL] [aA] [nN] [gG] [uU] [aA] [gG] [eE];
    LARGE: [lL] [aA] [rR] [gG] [eE];
    LAST: [lL] [aA] [sS] [tT];
    LEAKPROOF: [lL] [eE] [aA] [kK] [pP] [rR] [oO] [oO] [fF];
    LEVEL: [lL] [eE] [vV] [eE] [lL];
    LISTEN: [lL] [iI] [sS] [tT] [eE] [nN];
    LOAD: [lL] [oO] [aA] [dD];
    LOCAL: [lL] [oO] [cC] [aA] [lL];
    LOCATION: [lL] [oO] [cC] [aA] [tT] [iI] [oO] [nN];
    LOCK: [lL] [oO] [cC] [kK];
    LOCKED: [lL] [oO] [cC] [kK] [eE] [dD];
    LOGGED: [lL] [oO] [gG] [gG] [eE] [dD];

    MAPPING: [mM] [aA] [pP] [pP] [iI] [nN] [gG];
    MATCH: [mM] [aA] [tT] [cC] [hH];
    MATCHED: [mM] [aA] [tT] [cC] [hH] [eE] [dD];
    MATERIALIZED: [mM] [aA] [tT] [eE] [rR] [iI] [aA] [lL] [iI] [zZ] [eE] [dD];
    MAXVALUE: [mM] [aA] [xX] [vV] [aA] [lL] [uU] [eE];
    MERGE: [mM] [eE] [rR] [gG] [eE];
    METHOD: [mM] [eE] [tT] [hH] [oO] [dD];
    MINUTE: [mM] [iI] [nN] [uU] [tT] [eE];
    MINVALUE: [mM] [iI] [nN] [vV] [aA] [lL] [uU] [eE];
    MODE: [mM] [oO] [dD] [eE];
    MONTH: [mM] [oO] [nN] [tT] [hH];
    MOVE: [mM] [oO] [vV] [eE];

    NAME: [nN] [aA] [mM] [eE];
    NAMES: [nN] [aA] [mM] [eE] [sS];
    NEW: [nN] [eE] [wW];
    NEXT: [nN] [eE] [xX] [tT];
    NFC: [nN] [fF] [cC];
    NFD: [nN] [fF] [dD];
    NFKC: [nN] [fF] [kK] [cC];
    NFKD: [nN] [fF] [kK] [dD];
    NO: [nN] [oO];
    NORMALIZED: [nN] [oO] [rR] [mM] [aA] [lL] [iI] [zZ] [eE] [dD];
    NOTHING: [nN] [oO] [tT] [hH] [iI] [nN] [gG];
    NOTIFY: [nN] [oO] [tT] [iI] [fF] [yY];
    NOWAIT: [nN] [oO] [wW] [aA] [iI] [tT];
    NULLS: [nN] [uU] [lL] [lL] [sS];

    OBJECT: [oO] [bB] [jJ] [eE] [cC] [tT];
    OF: [oO] [fF];
    OFF: [oO] [fF] [fF];
    OIDS: [oO] [iI] [dD] [sS];
    OLD: [oO] [lL] [dD];
    OPERATOR: [oO] [pP] [eE] [rR] [aA] [tT] [oO] [rR];
    OPTION: [oO] [pP] [tT] [iI] [oO] [nN];
    OPTIONS: [oO] [pP] [tT] [iI] [oO] [nN] [sS];
    ORDINALITY: [oO] [rR] [dD] [iI] [nN] [aA] [lL] [iI] [tT] [yY];
    OTHERS: [oO] [tT] [hH] [eE] [rR] [sS];
    OVER: [oO] [vV] [eE] [rR];
    OVERRIDING: [oO] [vV] [eE] [rR] [rR] [iI] [dD] [iI] [nN] [gG];
    OWNED: [oO] [wW] [nN] [eE] [dD];
    OWNER: [oO] [wW] [nN] [eE] [rR];

    PARALLEL: [pP] [aA] [rR] [aA] [lL] [lL] [eE] [lL];
    PARSER: [pP] [aA] [rR] [sS] [eE] [rR];
    PARTIAL: [pP] [aA] [rR] [tT] [iI] [aA] [lL];
    PARTITION: [pP] [aA] [rR] [tT] [iI] [tT] [iI] [oO] [nN];
    PASSING: [pP] [aA] [sS] [sS] [iI] [nN] [gG];
    PASSWORD: [pP] [aA] [sS] [sS] [wW] [oO] [rR] [dD];
    PLANS: [pP] [lL] [aA] [nN] [sS];
    POLICY: [pP] [oO] [lL] [iI] [cC] [yY];
    PRECEDING: [pP] [rR] [eE] [cC] [eE] [dD] [iI] [nN] [gG];
    PREPARE: [pP] [rR] [eE] [pP] [aA] [rR] [eE];
    PREPARED: [pP] [rR] [eE] [pP] [aA] [rR] [eE] [dD];
    PRESERVE: [pP] [rR] [eE] [sS] [eE] [rR] [vV] [eE];
    PRIOR: [pP] [rR] [iI] [oO] [rR];
    PRIVILEGES: [pP] [rR] [iI] [vV] [iI] [lL] [eE] [gG] [eE] [sS];
    PROCEDURAL: [pP] [rR] [oO] [cC] [eE] [dD] [uU] [rR] [aA] [lL];
    PROCEDURE: [pP] [rR] [oO] [cC] [eE] [dD] [uU] [rR] [eE];
    PROCEDURES: [pP] [rR] [oO] [cC] [eE] [dD] [uU] [rR] [eE] [sS];
    PROGRAM: [pP] [rR] [oO] [gG] [rR] [aA] [mM];
    PUBLICATION: [pP] [uU] [bB] [lL] [iI] [cC] [aA] [tT] [iI] [oO] [nN];

    QUOTE: [qQ] [uU] [oO] [tT] [eE];

    RANGE: [rR] [aA] [nN] [gG] [eE];
    READ: [rR] [eE] [aA] [dD];
    REASSIGN: [rR] [eE] [aA] [sS] [sS] [iI] [gG] [nN];
    RECHECK: [rR] [eE] [cC] [hH] [eE] [cC] [kK];
    RECURSIVE: [rR] [eE] [cC] [uU] [rR] [sS] [iI] [vV] [eE];
    REF: [rR] [eE] [fF];
    REFERENCING: [rR] [eE] [fF] [eE] [rR] [eE] [nN] [cC] [iI] [nN] [gG];
    REFRESH: [rR] [eE] [fF] [rR] [eE] [sS] [hH];
    REINDEX: [rR] [eE] [iI] [nN] [dD] [eE] [xX];
    RELATIVE: [rR] [eE] [lL] [aA] [tT] [iI] [vV] [eE];
    RELEASE: [rR] [eE] [lL] [eE] [aA] [sS] [eE];
    RENAME: [rR] [eE] [nN] [aA] [mM] [eE];
    REPEATABLE: [rR] [eE] [pP] [eE] [aA] [tT] [aA] [bB] [lL] [eE];
    REPLACE: [rR] [eE] [pP] [lL] [aA] [cC] [eE];
    REPLICA: [rR] [eE] [pP] [lL] [iI] [cC] [aA];
    RESET: [rR] [eE] [sS] [eE] [tT];
    RESTART: [rR] [eE] [sS] [tT] [aA] [rR] [tT];
    RESTRICT: [rR] [eE] [sS] [tT] [rR] [iI] [cC] [tT];
    RETURN: [rR] [eE] [tT] [uU] [rR] [nN];
    RETURNS: [rR] [eE] [tT] [uU] [rR] [nN] [sS];
    REVOKE: [rR] [eE] [vV] [oO] [kK] [eE];
    ROLE: [rR] [oO] [lL] [eE];
    ROLLBACK: [rR] [oO] [lL] [lL] [bB] [aA] [cC] [kK];
    ROLLUP: [rR] [oO] [lL] [lL] [uU] [pP];
    ROUTINE: [rR] [oO] [uU] [tT] [iI] [nN] [eE];
    ROUTINES: [rR] [oO] [uU] [tT] [iI] [nN] [eE] [sS];
    ROWS: [rR] [oO] [wW] [sS];
    RULE: [rR] [uU] [lL] [eE];

    SAVEPOINT: [sS] [aA] [vV] [eE] [pP] [oO] [iI] [nN] [tT];
    SCHEMA: [sS] [cC] [hH] [eE] [mM] [aA];
    SCHEMAS: [sS] [cC] [hH] [eE] [mM] [aA] [sS];
    SCROLL: [sS] [cC] [rR] [oO] [lL] [lL];
    SEARCH: [sS] [eE] [aA] [rR] [cC] [hH];
    SECOND: [sS] [eE] [cC] [oO] [nN] [dD];
    SECURITY: [sS] [eE] [cC] [uU] [rR] [iI] [tT] [yY];
    SEQUENCE: [sS] [eE] [qQ] [uU] [eE] [nN] [cC] [eE];
    SEQUENCES: [sS] [eE] [qQ] [uU] [eE] [nN] [cC] [eE] [sS];
    SERIALIZABLE: [sS] [eE] [rR] [iI] [aA] [lL] [iI] [zZ] [aA] [bB] [lL] [eE];
    SERVER: [sS] [eE] [rR] [vV] [eE] [rR];
    SESSION: [sS] [eE] [sS] [sS] [iI] [oO] [nN];
    SET: [sS] [eE] [tT];
    SETS: [sS] [eE] [tT] [sS];
    SHARE: [sS] [hH] [aA] [rR] [eE];
    SHOW: [sS] [hH] [oO] [wW];
    SIMPLE: [sS] [iI] [mM] [pP] [lL] [eE];
    // SKIP is reserved by ANTLR
    SKIP_: [sS] [kK] [iI] [pP];
    SNAPSHOT: [sS] [nN] [aA] [pP] [sS] [hH] [oO] [tT];
    SQL: [sS] [qQ] [lL];
    STABLE: [sS] [tT] [aA] [bB] [lL] [eE];
    STANDALONE: [sS] [tT] [aA] [nN] [dD] [aA] [lL] [oO] [nN] [eE];
    START: [sS] [tT] [aA] [rR] [tT];
    STATEMENT: [sS] [tT] [aA] [tT] [eE] [mM] [eE] [nN] [tT];
    STATISTICS: [sS] [tT] [aA] [tT] [iI] [sS] [tT] [iI] [cC] [sS];
    STDIN: [sS] [tT] [dD] [iI] [nN];
    STDOUT: [sS] [tT] [dD] [oO] [uU] [tT];
    STORAGE: [sS] [tT] [oO] [rR] [aA] [gG] [eE];
    STORED: [sS] [tT] [oO] [rR] [eE] [dD];
    STRICT: [sS] [tT] [rR] [iI] [cC] [tT];
    STRIP: [sS] [tT] [rR] [iI] [pP];
    SUBSCRIPTION: [sS] [uU] [bB] [sS] [cC] [rR] [iI] [pP] [tT] [iI] [oO] [nN];
    SUPPORT: [sS] [uU] [pP] [pP] [oO] [rR] [tT];
    SYSID: [sS] [yY] [sS] [iI] [dD];
    SYSTEM: [sS] [yY] [sS] [tT] [eE] [mM];

    TABLES: [tT] [aA] [bB] [lL] [eE] [sS];
    TABLESPACE: [tT] [aA] [bB] [lL] [eE] [sS] [pP] [aA] [cC] [eE];
    TEMP: [tT] [eE] [mM] [pP];
    TEMPLATE: [tT] [eE] [mM] [pP] [lL] [aA] [tT] [eE];
    TEMPORARY: [tT] [eE] [mM] [pP] [oO] [rR] [aA] [rR] [yY];
    TEXT: [tT] [eE] [xX] [tT];
    TIES: [tT] [iI] [eE] [sS];
    TRANSACTION: [tT] [rR] [aA] [nN] [sS] [aA] [cC] [tT] [iI] [oO] [nN];
    TRANSFORM: [tT] [rR] [aA] [nN] [sS] [fF] [oO] [rR] [mM];
    TRIGGER: [tT] [rR] [iI] [gG] [gG] [eE] [rR];
    TRUNCATE: [tT] [rR] [uU] [nN] [cC] [aA] [tT] [eE];
    TRUSTED: [tT] [rR] [uU] [sS] [tT] [eE] [dD];
    TYPE: [tT] [yY] [pP] [eE];
    TYPES: [tT] [yY] [pP] [eE] [sS];

    UESCAPE: [uU] [eE] [sS] [cC] [aA] [pP] [eE];
    UNBOUNDED: [uU] [nN] [bB] [oO] [uU] [nN] [dD] [eE] [dD];
    UNCOMMITTED: [uU] [nN] [cC] [oO] [mM] [mM] [iI] [tT] [tT] [eE] [dD];
    UNENCRYPTED: [uU] [nN] [eE] [nN] [cC] [rR] [yY] [pP] [tT] [eE] [dD];
    UNKNOWN: [uU] [nN] [kK] [nN] [oO] [wW] [nN];
    UNLISTEN: [uU] [nN] [lL] [iI] [sS] [tT] [eE] [nN];
    UNLOGGED: [uU] [nN] [lL] [oO] [gG] [gG] [eE] [dD];
    UNTIL: [uU] [nN] [tT] [iI] [lL];
    UPDATE: [uU] [pP] [dD] [aA] [tT] [eE];

    VACUUM: [vV] [aA] [cC] [uU] [uU] [mM];
    VALID: [vV] [aA] [lL] [iI] [dD];
    VALIDATE: [vV] [aA] [lL] [iI] [dD] [aA] [tT] [eE];
    VALIDATOR: [vV] [aA] [lL] [iI] [dD] [aA] [tT] [oO] [rR];
    VALUE: [vV] [aA] [lL] [uU] [eE];
    VARYING: [vV] [aA] [rR] [yY] [iI] [nN] [gG];
    VERSION: [vV] [eE] [rR] [sS] [iI] [oO] [nN];
    VIEW: [vV] [iI] [eE] [wW];
    VIEWS: [vV] [iI] [eE] [wW] [sS];
    VOLATILE: [vV] [oO] [lL] [aA] [tT] [iI] [lL] [eE];

    WHITESPACE: [wW] [hH] [iI] [tT] [eE] [sS] [pP] [aA] [cC] [eE];
    WITHIN: [wW] [iI] [tT] [hH] [iI] [nN];
    WITHOUT: [wW] [iI] [tT] [hH] [oO] [uU] [tT];
    WORK: [wW] [oO] [rR] [kK];
    WRAPPER: [wW] [rR] [aA] [pP] [pP] [eE] [rR];
    WRITE: [wW] [rR] [iI] [tT] [eE];

    XML: [xX] [mM] [lL];

    YEAR: [yY] [eE] [aA] [rR];
    YES: [yY] [eE] [sS];

    ZONE: [zZ] [oO] [nN] [eE];

    /*
    ==================================================
    COL_NAME_KEYWORD
    ==================================================
    */

    BETWEEN: [bB] [eE] [tT] [wW] [eE] [eE] [nN];
    BIGINT: [bB] [iI] [gG] [iI] [nN] [tT];
    BIT: [bB] [iI] [tT];
    BOOLEAN: [bB] [oO] [oO] [lL] [eE] [aA] [nN];

    CHAR: [cC] [hH] [aA] [rR];
    CHARACTER: [cC] [hH] [aA] [rR] [aA] [cC] [tT] [eE] [rR];
    COALESCE: [cC] [oO] [aA] [lL] [eE] [sS] [cC] [eE];

    DEC: [dD] [eE] [cC];
    DECIMAL: [dD] [eE] [cC] [iI] [mM] [aA] [lL];

    EXISTS: [eE] [xX] [iI] [sS] [tT] [sS];
    EXTRACT: [eE] [xX] [tT] [rR] [aA] [cC] [tT];

    FLOAT: [fF] [lL] [oO] [aA] [tT];

    GREATEST: [gG] [rR] [eE] [aA] [tT] [eE] [sS] [tT];
    GROUPING: [gG] [rR] [oO] [uU] [pP] [iI] [nN] [gG];

    INOUT: [iI] [nN] [oO] [uU] [tT];
    INT: [iI] [nN] [tT];
    INTEGER: [iI] [nN] [tT] [eE] [gG] [eE] [rR];
    INTERVAL: [iI] [nN] [tT] [eE] [rR] [vV] [aA] [lL];

    LEAST: [lL] [eE] [aA] [sS] [tT];

    NATIONAL: [nN] [aA] [tT] [iI] [oO] [nN] [aA] [lL];
    NCHAR: [nN] [cC] [hH] [aA] [rR];
    NONE: [nN] [oO] [nN] [eE];
    NORMALIZE: [nN] [oO] [rR] [mM] [aA] [lL] [iI] [zZ] [eE];
    NULLIF: [nN] [uU] [lL] [lL] [iI] [fF];
    NUMERIC: [nN] [uU] [mM] [eE] [rR] [iI] [cC];

    OUT: [oO] [uU] [tT];
    OVERLAY: [oO] [vV] [eE] [rR] [lL] [aA] [yY];

    POSITION: [pP] [oO] [sS] [iI] [tT] [iI] [oO] [nN];
    PRECISION: [pP] [rR] [eE] [cC] [iI] [sS] [iI] [oO] [nN];

    REAL: [rR] [eE] [aA] [lL];
    ROW: [rR] [oO] [wW];

    SETOF: [sS] [eE] [tT] [oO] [fF];
    SMALLINT: [sS] [mM] [aA] [lL] [lL] [iI] [nN] [tT];
    SUBSTRING: [sS] [uU] [bB] [sS] [tT] [rR] [iI] [nN] [gG];

    TIME: [tT] [iI] [mM] [eE];
    TIMESTAMP: [tT] [iI] [mM] [eE] [sS] [tT] [aA] [mM] [pP];
    TREAT: [tT] [rR] [eE] [aA] [tT];
    TRIM: [tT] [rR] [iI] [mM];

    VALUES: [vV] [aA] [lL] [uU] [eE] [sS];
    VARCHAR: [vV] [aA] [rR] [cC] [hH] [aA] [rR];

    XMLATTRIBUTES: [xX] [mM] [lL] [aA] [tT] [tT] [rR] [iI] [bB] [uU] [tT] [eE] [sS];
    XMLCONCAT: [xX] [mM] [lL] [cC] [oO] [nN] [cC] [aA] [tT];
    XMLELEMENT: [xX] [mM] [lL] [eE] [lL] [eE] [mM] [eE] [nN] [tT];
    XMLEXISTS: [xX] [mM] [lL] [eE] [xX] [iI] [sS] [tT] [sS];
    XMLFOREST: [xX] [mM] [lL] [fF] [oO] [rR] [eE] [sS] [tT];
    XMLNAMESPACES: [xX] [mM] [lL] [nN] [aA] [mM] [eE] [sS] [pP] [aA] [cC] [eE] [sS];
    XMLPARSE: [xX] [mM] [lL] [pP] [aA] [rR] [sS] [eE];
    XMLPI: [xX] [mM] [lL] [pP] [iI];
    XMLROOT: [xX] [mM] [lL] [rR] [oO] [oO] [tT];
    XMLSERIALIZE: [xX] [mM] [lL] [sS] [eE] [rR] [iI] [aA] [lL] [iI] [zZ] [eE];
    XMLTABLE: [xX] [mM] [lL] [tT] [aA] [bB] [lL] [eE];

    /*
    ==================================================
    TYPE_FUNC_NAME_KEYWORD
    ==================================================
    */

    AUTHORIZATION: [aA] [uU] [tT] [hH] [oO] [rR] [iI] [zZ] [aA] [tT] [iI] [oO] [nN];

    BINARY: [bB] [iI] [nN] [aA] [rR] [yY];

    COLLATION: [cC] [oO] [lL] [lL] [aA] [tT] [iI] [oO] [nN];
    CONCURRENTLY: [cC] [oO] [nN] [cC] [uU] [rR] [rR] [eE] [nN] [tT] [lL] [yY];
    CROSS: [cC] [rR] [oO] [sS] [sS];
    CURRENT_SCHEMA: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [sS] [cC] [hH] [eE] [mM] [aA];

    FREEZE: [fF] [rR] [eE] [eE] [zZ] [eE];
    FULL: [fF] [uU] [lL] [lL];

    ILIKE: [iI] [lL] [iI] [kK] [eE];
    INNER: [iI] [nN] [nN] [eE] [rR];
    IS: [iI] [sS];
    ISNULL: [iI] [sS] [nN] [uU] [lL] [lL];

    JOIN: [jJ] [oO] [iI] [nN];

    LEFT: [lL] [eE] [fF] [tT];
    LIKE: [lL] [iI] [kK] [eE];

    NATURAL: [nN] [aA] [tT] [uU] [rR] [aA] [lL];
    NOTNULL: [nN] [oO] [tT] [nN] [uU] [lL] [lL];

    OUTER: [oO] [uU] [tT] [eE] [rR];
    OVERLAPS: [oO] [vV] [eE] [rR] [lL] [aA] [pP] [sS];

    RIGHT: [rR] [iI] [gG] [hH] [tT];

    SIMILAR: [sS] [iI] [mM] [iI] [lL] [aA] [rR];

    TABLESAMPLE: [tT] [aA] [bB] [lL] [eE] [sS] [aA] [mM] [pP] [lL] [eE];

    VERBOSE: [vV] [eE] [rR] [bB] [oO] [sS] [eE];

    /*
    ==================================================
    RESERVED_KEYWORD
    ==================================================
    */

    ALL: [aA] [lL] [lL];   // first RESERVED_KEYWORD, sync with AntlrUtils.normalizeWhitespaceUnquoted
    ANALYZE: [aA] [nN] [aA] [lL] [yY] [zZsS] [eE];
    AND: [aA] [nN] [dD];
    ANY: [aA] [nN] [yY];
    ARRAY: [aA] [rR] [rR] [aA] [yY];
    AS: [aA] [sS];
    ASC: [aA] [sS] [cC];
    ASYMMETRIC: [aA] [sS] [yY] [mM] [mM] [eE] [tT] [rR] [iI] [cC];

    BOTH: [bB] [oO] [tT] [hH];

    CASE: [cC] [aA] [sS] [eE];
    CAST: [cC] [aA] [sS] [tT];
    CHECK: [cC] [hH] [eE] [cC] [kK];
    COLLATE: [cC] [oO] [lL] [lL] [aA] [tT] [eE];
    COLUMN: [cC] [oO] [lL] [uU] [mM] [nN];
    CONSTRAINT: [cC] [oO] [nN] [sS] [tT] [rR] [aA] [iI] [nN] [tT];
    CREATE: [cC] [rR] [eE] [aA] [tT] [eE];
    CURRENT_CATALOG: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [cC] [aA] [tT] [aA] [lL] [oO] [gG];
    CURRENT_DATE: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [dD] [aA] [tT] [eE];
    CURRENT_ROLE: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [rR] [oO] [lL] [eE];
    CURRENT_TIME: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [tT] [iI] [mM] [eE];
    CURRENT_TIMESTAMP: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [tT] [iI] [mM] [eE] [sS] [tT] [aA] [mM] [pP];
    CURRENT_USER: [cC] [uU] [rR] [rR] [eE] [nN] [tT] UNDERLINE  [uU] [sS] [eE] [rR];

    DEFAULT: [dD] [eE] [fF] [aA] [uU] [lL] [tT];
    DEFERRABLE: [dD] [eE] [fF] [eE] [rR] [rR] [aA] [bB] [lL] [eE];
    DESC: [dD] [eE] [sS] [cC];
    DISTINCT: [dD] [iI] [sS] [tT] [iI] [nN] [cC] [tT];
    DO: [dD] [oO];

    ELSE: [eE] [lL] [sS] [eE];
    END: [eE] [nN] [dD];
    EXCEPT: [eE] [xX] [cC] [eE] [pP] [tT];

    FALSE: [fF] [aA] [lL] [sS] [eE];
    FETCH: [fF] [eE] [tT] [cC] [hH];
    FOR: [fF] [oO] [rR];
    FOREIGN: [fF] [oO] [rR] [eE] [iI] [gG] [nN];
    FROM: [fF] [rR] [oO] [mM];

    GRANT: [gG] [rR] [aA] [nN] [tT];
    GROUP: [gG] [rR] [oO] [uU] [pP];

    HAVING: [hH] [aA] [vV] [iI] [nN] [gG];

    IN: [iI] [nN];
    INITIALLY: [iI] [nN] [iI] [tT] [iI] [aA] [lL] [lL] [yY];
    INTERSECT: [iI] [nN] [tT] [eE] [rR] [sS] [eE] [cC] [tT];
    INTO: [iI] [nN] [tT] [oO];

    LATERAL: [lL] [aA] [tT] [eE] [rR] [aA] [lL];
    LEADING: [lL] [eE] [aA] [dD] [iI] [nN] [gG];
    LIMIT: [lL] [iI] [mM] [iI] [tT];
    LOCALTIME: [lL] [oO] [cC] [aA] [lL] [tT] [iI] [mM] [eE];
    LOCALTIMESTAMP: [lL] [oO] [cC] [aA] [lL] [tT] [iI] [mM] [eE] [sS] [tT] [aA] [mM] [pP];

    NOT: [nN] [oO] [tT];
    NULL: [nN] [uU] [lL] [lL];

    OFFSET: [oO] [fF] [fF] [sS] [eE] [tT];
    ON: [oO] [nN];
    ONLY: [oO] [nN] [lL] [yY];
    OR: [oO] [rR];
    ORDER: [oO] [rR] [dD] [eE] [rR];

    PLACING: [pP] [lL] [aA] [cC] [iI] [nN] [gG];
    PRIMARY: [pP] [rR] [iI] [mM] [aA] [rR] [yY];

    REFERENCES: [rR] [eE] [fF] [eE] [rR] [eE] [nN] [cC] [eE] [sS];
    RETURNING: [rR] [eE] [tT] [uU] [rR] [nN] [iI] [nN] [gG];

    SELECT: [sS] [eE] [lL] [eE] [cC] [tT];
    SESSION_USER: [sS] [eE] [sS] [sS] [iI] [oO] [nN] UNDERLINE  [uU] [sS] [eE] [rR];
    SOME: [sS] [oO] [mM] [eE];
    SYMMETRIC: [sS] [yY] [mM] [mM] [eE] [tT] [rR] [iI] [cC];

    TABLE: [tT] [aA] [bB] [lL] [eE];
    THEN: [tT] [hH] [eE] [nN];
    TO: [tT] [oO];
    TRAILING: [tT] [rR] [aA] [iI] [lL] [iI] [nN] [gG];
    TRUE: [tT] [rR] [uU] [eE];

    UNION: [uU] [nN] [iI] [oO] [nN];
    UNIQUE: [uU] [nN] [iI] [qQ] [uU] [eE];
    USER: [uU] [sS] [eE] [rR];
    USING: [uU] [sS] [iI] [nN] [gG];

    VARIADIC: [vV] [aA] [rR] [iI] [aA] [dD] [iI] [cC];

    WHEN: [wW] [hH] [eE] [nN];
    WHERE: [wW] [hH] [eE] [rR] [eE];
    WINDOW: [wW] [iI] [nN] [dD] [oO] [wW];
    WITH: [wW] [iI] [tT] [hH];   // last RESERVED_KEYWORD, sync with AntlrUtils.normalizeWhitespaceUnquoted

    /*
     * Other tokens.
     * Some sql words/data types are not keywords but we need a token to be able to parse them.
     *
     * Manually added word-tokens must also be manually added to the
     * tokens_nonkeyword parser rule SQLParser.g4.
     */

    ALIGNMENT: [aA] [lL] [iI] [gG] [nN] [mM] [eE] [nN] [tT];
    ALLOW_CONNECTIONS: [aA] [lL] [lL] [oO] [wW] UNDERLINE [cC] [oO] [nN] [nN] [eE] [cC] [tT] [iI] [oO] [nN] [sS];

    BASETYPE: [bB] [aA] [sS] [eE] [tT] [yY] [pP] [eE];
    BUFFERS: [bB] [uU] [fF] [fF] [eE] [rR] [sS];
    BYPASSRLS: [bB] [yY] [pP] [aA] [sS] [sS] [rR] [lL] [sS];

    CANONICAL: [cC] [aA] [nN] [oO] [nN] [iI] [cC] [aA] [lL];
    CATEGORY: [cC] [aA] [tT] [eE] [gG] [oO] [rR] [yY];
    COLLATABLE: [cC] [oO] [lL] [lL] [aA] [tT] [aA] [bB] [lL] [eE];
    COMBINEFUNC: [cC] [oO] [mM] [bB] [iI] [nN] [eE] [fF] [uU] [nN] [cC];
    COMMUTATOR: [cC] [oO] [mM] [mM] [uU] [tT] [aA] [tT] [oO] [rR];
    CONNECT: [cC] [oO] [nN] [nN] [eE] [cC] [tT];
    COSTS: [cC] [oO] [sS] [tT] [sS];
    CREATEDB: [cC] [rR] [eE] [aA] [tT] [eE] [dD] [bB];
    CREATEROLE: [cC] [rR] [eE] [aA] [tT] [eE] [rR] [oO] [lL] [eE];

    DESERIALFUNC: [dD] [eE] [sS] [eE] [rR] [iI] [aA] [lL] [fF] [uU] [nN] [cC];
    DETERMINISTIC: [dD] [eE] [tT] [eE] [rR] [mM] [iI] [nN] [iI] [sS] [tT] [iI] [cC];
    DISABLE_PAGE_SKIPPING: DISABLE UNDERLINE [pP] [aA] [gG] [eE] UNDERLINE [sS] [kK] [iI] [pP] [pP] [iI] [nN] [gG];

    ELEMENT: [eE] [lL] [eE] [mM] [eE] [nN] [tT];
    EXTENDED: [eE] [xX] [tT] [eE] [nN] [dD] [eE] [dD];

    FINALFUNC: [fF] [iI] [nN] [aA] [lL] [fF] [uU] [nN] [cC];
    FINALFUNC_EXTRA: FINALFUNC UNDERLINE  [eE] [xX] [tT] [rR] [aA];
    FINALFUNC_MODIFY: FINALFUNC UNDERLINE [mM] [oO] [dD] [iI] [fF] [yY];
    FORCE_NOT_NULL: [fF] [oO] [rR] [cC] [eE] UNDERLINE [nN] [oO] [tT] UNDERLINE [nN] [uU] [lL] [lL];
    FORCE_NULL: [fF] [oO] [rR] [cC] [eE] UNDERLINE [nN] [uU] [lL] [lL];
    FORCE_QUOTE: [fF] [oO] [rR] [cC] [eE] UNDERLINE [qQ] [uU] [oO] [tT] [eE];
    FORMAT: [fF] [oO] [rR] [mM] [aA] [tT];

    GETTOKEN: [gG] [eE] [tT] [tT] [oO] [kK] [eE] [nN];
    GTCMP: [gG] [tT] [cC] [mM] [pP];

    HASH: [hH] [aA] [sS] [hH];
    HASHES: [hH] [aA] [sS] [hH] [eE] [sS];
    HEADLINE: [hH] [eE] [aA] [dD] [lL] [iI] [nN] [eE];
    HYPOTHETICAL: [hH] [yY] [pP] [oO] [tT] [hH] [eE] [tT] [iI] [cC] [aA] [lL];

    INDEX_CLEANUP: [iI] [nN] [dD] [eE] [xX] UNDERLINE [cC] [lL] [eE] [aA] [nN] [uU] [pP];
    INIT: [iI] [nN] [iI] [tT];
    INITCOND: [iI] [nN] [iI] [tT] [cC] [oO] [nN] [dD];
    INITCOND1: [iI] [nN] [iI] [tT] [cC] [oO] [nN] [dD] '1';
    INITCOND2: [iI] [nN] [iI] [tT] [cC] [oO] [nN] [dD] '2';
    INTERNALLENGTH: [iI] [nN] [tT] [eE] [rR] [nN] [aA] [lL] [lL] [eE] [nN] [gG] [tT] [hH];
    IS_TEMPLATE: [iI] [sS] UNDERLINE [tT] [eE] [mM] [pP] [lL] [aA] [tT] [eE];

    JSON: [jJ] [sS] [oO] [nN];

    LC_COLLATE: [lL] [cC] UNDERLINE [cC] [oO] [lL] [lL] [aA] [tT] [eE];
    LC_CTYPE: [lL] [cC] UNDERLINE [cC] [tT] [yY] [pP] [eE];
    LEFTARG: [lL] [eE] [fF] [tT] [aA] [rR] [gG];
    LEXIZE: [lL] [eE] [xX] [iI] [zZ] [eE];
    LEXTYPES: [lL] [eE] [xX] [tT] [yY] [pP] [eE] [sS];
    LIST: [lL] [iI] [sS] [tT];
    LOCALE: [lL] [oO] [cC] [aA] [lL] [eE];
    LOGIN: [lL] [oO] [gG] [iI] [nN];
    LTCMP: [lL] [tT] [cC] [mM] [pP];

    MAIN: [mM] [aA] [iI]  [nN];
    MERGES: [mM] [eE] [rR] [gG] [eE] [sS];
    MFINALFUNC: [mM] [fF] [iI] [nN] [aA] [lL] [fF] [uU] [nN] [cC];
    MFINALFUNC_EXTRA: MFINALFUNC UNDERLINE [eE] [xX] [tT] [rR] [aA];
    MFINALFUNC_MODIFY: MFINALFUNC UNDERLINE [mM] [oO] [dD] [iI] [fF] [yY];
    MINITCOND: [mM] [iI] [nN] [iI] [tT] [cC] [oO] [nN] [dD];
    MINVFUNC: [mM] [iI] [nN] [vV] [fF] [uU] [nN] [cC];
    MODULUS: [mM] [oO] [dD] [uU] [lL] [uU] [sS];
    MSFUNC: [mM] [sS] [fF] [uU] [nN] [cC];
    MSSPACE: [mM] [sS] [sS] [pP] [aA] [cC] [eE];
    MSTYPE: [mM] [sS] [tT] [yY] [pP] [eE];
    MULTIRANGE_TYPE_NAME: [mM] [uU] [lL] [tT] [iI] [rR] [aA] [nN] [gG] [eE] UNDERLINE [tT] [yY] [pP] [eE] UNDERLINE [nN] [aA] [mM] [eE] ;

    NEGATOR: [nN] [eE] [gG] [aA] [tT] [oO] [rR];
    NOBYPASSRLS: [nN] [oO] [bB] [yY] [pP] [aA] [sS] [sS] [rR] [lL] [sS];
    NOCREATEDB: [nN] [oO] [cC] [rR] [eE] [aA] [tT] [eE] [dD] [bB];
    NOCREATEROLE: [nN] [oO] [cC] [rR] [eE] [aA] [tT] [eE] [rR] [oO] [lL] [eE];
    NOINHERIT: [nN] [oO] [iI] [nN] [hH] [eE] [rR] [iI] [tT];
    NOLOGIN: [nN] [oO] [lL] [oO] [gG] [iI] [nN];
    NOREPLICATION: [nN] [oO] [rR] [eE] [pP] [lL] [iI] [cC] [aA] [tT] [iI] [oO] [nN];
    NOSUPERUSER: [nN] [oO] [sS] [uU] [pP] [eE] [rR] [uU] [sS] [eE] [rR];

    OUTPUT: [oO] [uU] [tT] [pP] [uU] [tT];

    PASSEDBYVALUE: [pP] [aA] [sS] [sS] [eE] [dD] [bB] [yY] [vV] [aA] [lL] [uU] [eE];
    PATH: [pP] [aA] [tT] [hH];
    PERMISSIVE: [pP] [eE] [rR] [mM] [iI] [sS] [sS] [iI] [vV] [eE];
    PLAIN: [pP] [lL] [aA] [iI]  [nN];
    PREFERRED: [pP] [rR] [eE] [fF] [eE] [rR] [rR] [eE] [dD];
    PROVIDER: [pP] [rR] [oO] [vV] [iI] [dD] [eE] [rR];

    READ_ONLY: READ UNDERLINE ONLY;
    READ_WRITE: READ UNDERLINE WRITE;
    RECEIVE: [rR] [eE] [cC] [eE] [iI] [vV] [eE];
    REMAINDER: [rR] [eE] [mM] [aA] [iI] [nN] [dD] [eE] [rR];
    REPLICATION: [rR] [eE] [pP] [lL] [iI] [cC] [aA] [tT] [iI] [oO] [nN];
    RESTRICTED: [rR] [eE] [sS] [tT] [rR] [iI] [cC] [tT] [eE] [dD];
    RESTRICTIVE: [rR] [eE] [sS] [tT] [rR] [iI] [cC] [tT] [iI] [vV] [eE];
    RIGHTARG: [rR] [iI] [gG] [hH] [tT] [aA] [rR] [gG];

    SAFE: [sS] [aA] [fF] [eE];
    SEND: [sS] [eE] [nN] [dD];
    SERIALFUNC: [sS] [eE] [rR] [iI] [aA] [lL] [fF] [uU] [nN] [cC];
    SETTINGS: [sS] [eE] [tT] [tT] [iI] [nN] [gG] [sS];
    SFUNC: [sS] [fF] [uU] [nN] [cC];
    SFUNC1: [sS] [fF] [uU] [nN] [cC] '1';
    SFUNC2: [sS] [fF] [uU] [nN] [cC] '2';
    SHAREABLE: [sS] [hH] [aA] [rR] [eE] [aA] [bB] [lL] [eE];
    SKIP_LOCKED: [sS] [kK] [iI] [pP] UNDERLINE [lL] [oO] [cC] [kK] [eE] [dD];
    SORT1: [sS] [oO] [rR] [tT] '1';
    SORT2: [sS] [oO] [rR] [tT] '2';
    SORTOP: [sS] [oO] [rR] [tT] [oO] [pP];
    SSPACE: [sS] [sS] [pP] [aA] [cC] [eE];
    STYPE: [sS] [tT] [yY] [pP] [eE];
    STYPE1: [sS] [tT] [yY] [pP] [eE] '1';
    STYPE2: [sS] [tT] [yY] [pP] [eE] '2';
    SUBTYPE_DIFF: [sS] [uU] [bB] [tT] [yY] [pP] [eE] UNDERLINE [dD] [iI] [fF] [fF];
    SUBTYPE_OPCLASS: [sS] [uU] [bB] [tT] [yY] [pP] [eE] UNDERLINE [oO] [pP] [cC] [lL] [aA] [sS] [sS];
    SUBTYPE: [sS] [uU] [bB] [tT] [yY] [pP] [eE];
    SUBSCRIPT: [sS] [uU] [bB] [sS] [cC] [rR] [iI] [pP] [tT];
    SUMMARY: [sS] [uU] [mM] [mM] [aA] [rR] [yY];
    SUPERUSER: [sS] [uU] [pP] [eE] [rR] [uU] [sS] [eE] [rR];

    TIMING: [tT] [iI] [mM] [iI] [nN] [gG];
    TYPMOD_IN: [tT] [yY] [pP] [mM] [oO] [dD] UNDERLINE [iI]  [nN];
    TYPMOD_OUT: [tT] [yY] [pP] [mM] [oO] [dD] UNDERLINE [oO] [uU] [tT];

    UNSAFE: [uU] [nN] [sS] [aA] [fF] [eE];
    USAGE: [uU] [sS] [aA] [gG] [eE];

    VARIABLE: [vV] [aA] [rR] [iI] [aA] [bB] [lL] [eE];

    WAL: [wW] [aA] [lL];

    YAML: [yY] [aA] [mM] [lL];

    // plpgsql tokens

    ALIAS: [aA] [lL] [iI] [aA] [sS];
    ASSERT: [aA] [sS] [sS] [eE] [rR] [tT];

    CONSTANT: [cC] [oO] [nN] [sS] [tT] [aA] [nN] [tT];

    DATATYPE: [dD] [aA] [tT] [aA] [tT] [yY] [pP] [eE];
    DEBUG: [dD] [eE] [bB] [uU] [gG];
    DETAIL: [dD] [eE] [tT] [aA] [iI] [lL];
    DIAGNOSTICS: [dD] [iI] [aA] [gG] [nN] [oO] [sS] [tT] [iI] [cC] [sS];

    ELSEIF: [eE] [lL] [sS] [eE] [iI] [fF];
    ELSIF: [eE] [lL] [sS] [iI] [fF];
    ERRCODE: [eE] [rR] [rR] [cC] [oO] [dD] [eE];
    EXIT: [eE] [xX] [iI] [tT];
    EXCEPTION: [eE] [xX] [cC] [eE] [pP] [tT] [iI] [oO] [nN];

    FOREACH: [fF] [oO] [rR] [eE] [aA] [cC] [hH];

    GET: [gG] [eE] [tT];

    HINT: [hH] [iI] [nN] [tT];

    INFO: [iI] [nN] [fF] [oO];

    LOG: [lL] [oO] [gG];
    LOOP: [lL] [oO] [oO] [pP];

    MESSAGE: [mM] [eE] [sS] [sS] [aA] [gG] [eE];

    NOTICE: [nN] [oO] [tT] [iI] [cC] [eE];

    OPEN: [oO] [pP] [eE] [nN];

    PERFORM: [pP] [eE] [rR] [fF] [oO] [rR] [mM];

    QUERY: [qQ] [uU] [eE] [rR] [yY];

    RAISE: [rR] [aA] [iI] [sS] [eE];
    RECORD: [rR] [eE] [cC] [oO] [rR] [dD];
    REVERSE: [rR] [eE] [vV] [eE] [rR] [sS] [eE];
    ROWTYPE: [rR] [oO] [wW] [tT] [yY] [pP] [eE];

    SLICE: [sS] [lL] [iI] [cC] [eE];
    SQLSTATE: [sS] [qQ] [lL] [sS] [tT] [aA] [tT] [eE];
    STACKED: [sS] [tT] [aA] [cC] [kK] [eE] [dD];

    WARNING: [wW] [aA] [rR] [nN] [iI] [nN] [gG];
    WHILE: [wW] [hH] [iI] [lL] [eE];     // last identifier rule, sync with CustomSQLAntlrErrorStrategy & AntlrUtils

fragment UNDERLINE : '_';

// Operators

// Cast Operator
CAST_EXPRESSION : ':' ':';   // first operator rule, sync with CustomSQLAntlrErrorStrategy

EQUAL : '=';
COLON :  ':';
SEMI_COLON :  ';';
COMMA : ',';
NOT_EQUAL : '<>' | '!=';
LTH : '<';
LEQ : '<=';
GTH : '>';
GEQ : '>=';
LEFT_PAREN :  '(';
RIGHT_PAREN : ')';
PLUS  : '+';
MINUS : '-';
MULTIPLY: '*';
DIVIDE  : '/';
MODULAR : '%';
EXP : '^';

DOT : '.';
QUOTE_CHAR : '\'';
DOUBLE_QUOTE : '"';
DOLLAR : '$';
LEFT_BRACKET : '[';
RIGHT_BRACKET : ']';

EQUAL_GTH : '=>';
COLON_EQUAL : ':=';

LESS_LESS : '<<';
GREATER_GREATER : '>>';
DOUBLE_DOT: '..';
HASH_SIGN: '#';              // last operator rule, sync with CustomSQLAntlrErrorStrategy

BlockComment
    :   '/*' (BlockComment |.)*? '*/' -> channel(HIDDEN)
    ;

DNDBT_ID_DECLARATION_COMMENT:
    '--ID:#{' ~[\r\n]* '}#' (('\r'? '\n') | EOF)
    ;

LineComment
    :   '--' ~[\r\n]* -> channel(HIDDEN)
    ;

// must follow all explicitly defined operators and comments
// so that they are matched first
OP_CHARS
    // may not end with + or -
    : OperatorBasic+ OperatorBasicEnd
    // may end with any character but must include at least one of OperatorSpecial
    | (OperatorBasic | OperatorSpecial)* OperatorSpecial (OperatorBasic | OperatorSpecial)*
    ;

fragment
OperatorBasic
    : [+*<>=]
    // check so that comment start sequences are not matched by this
    | '-' {InputStream.LA(1) != '-'}?
    | '/' {InputStream.LA(1) != '*'}?;
fragment
OperatorBasicEnd: [*/<>=];
fragment
OperatorSpecial: [~!@#%^&|`?];

NUMBER_LITERAL : Digit+;

fragment
Digit : '0'..'9';

REAL_NUMBER
    // fail double dots into a separate token
    // otherwise 1..10 would lex into 2 numbers
    :   Digit+ '.' {InputStream.LA(1) != '.'}?
    |   Digit+ '.' Digit+ EXPONENT?
    |   Digit+ '.' EXPONENT
    |   '.' Digit+ EXPONENT?
    |   Digit+ EXPONENT
    ;

DOLLAR_NUMBER
    : DOLLAR NUMBER_LITERAL
    ;

/*
===============================================================================
 Identifiers
===============================================================================
*/

Identifier
    : IdentifierStartChar IdentifierChar*
    // always lowercase unquoted ids
        { this.Text = this.Text.ToLower(System.Globalization.CultureInfo.InvariantCulture); }
    ;
fragment
IdentifierStartChar
    : // these are the valid identifier start characters below 0x7F
    [a-zA-Z_]
    | // these are the valid characters from 0x80 to 0xFF
    [\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u00FF]
    | // these are the letters above 0xFF which only need a single UTF-16 code unit
    [\u0100-\uD7FF\uE000-\uFFFF] {Char.IsLetter((char)InputStream.LA(-1))}?
    | // letters which require multiple UTF-16 code units
    [\uD800-\uDBFF] [\uDC00-\uDFFF] {Char.IsLetter(Char.ConvertFromUtf32(Char.ConvertToUtf32((char)InputStream.LA(-2), (char)InputStream.LA(-1))).Substring(0)[0])}?
    ;
fragment
IdentifierChar
    : StrictIdentifierChar
    | '$'
    ;
fragment
StrictIdentifierChar
    : IdentifierStartChar
    | [0-9]
    ;

/* Quoted Identifiers
*
* These are divided into four separate tokens, allowing distinction of valid quoted identifiers from invalid quoted
* identifiers without sacrificing the ability of the lexer to reliably recover from lexical errors in the input.
*/
QuotedIdentifier
    : UnterminatedQuotedIdentifier '"'
    ;
// This is a quoted identifier which only contains valid characters but is not terminated
fragment UnterminatedQuotedIdentifier
    : '"'
    ( '""' | ~[\u0000"] )*
    ;
/*
===============================================================================
 Literal
===============================================================================
*/

// Some Unicode Character Ranges
fragment
Control_Characters                  :   '\u0001' .. '\u0008' | '\u000B' | '\u000C' | '\u000E' .. '\u001F';
fragment
Extended_Control_Characters         :   '\u0080' .. '\u009F';

Character_String_Literal
    : [eEnNxXbB]? Single_String (String_Joiner Single_String)*
    ;

fragment
Single_String
    : QUOTE_CHAR ( ~'\'' | '\'\'')* QUOTE_CHAR
    ;

fragment
String_Joiner
    :  ((Space | Tab | White_Space | LineComment)* New_Line)+ (Space | Tab | White_Space)*
    ;

fragment
EXPONENT : ('e'|'E') ('+'|'-')? Digit+ ;

// Dollar-quoted String Constants (§4.1.2.4)
BeginDollarStringConstant
    : '$' Tag? '$' {_tags.Push(this.Text);} -> pushMode(DollarQuotedStringMode)
    ;

fragment
Tag
    : IdentifierStartChar StrictIdentifierChar*
    ;


/*
===============================================================================
 Whitespace Tokens
===============================================================================
*/

Space
  : ' ' -> channel(HIDDEN)
  ;

White_Space
  : ( Control_Characters  | Extended_Control_Characters )+ -> channel(HIDDEN)
  ;

New_Line
    : ('\u000D' | '\u000D'? '\u000A') -> channel(HIDDEN)
    ;

Tab
    : '\u0009' -> channel(HIDDEN)
    ;

BOM: '\ufeff';

BAD
  : .
  ;

mode DollarQuotedStringMode;
Text_between_Dollar
   : ~'$'+
    | // this alternative improves the efficiency of handling $ characters within a dollar-quoted string which are
    // not part of the ending tag.
    '$' ~'$'*
    ;

EndDollarStringConstant
    : '$' Tag? '$' {this.Text.Equals(_tags.Peek())}? {_tags.Pop();} -> popMode
    ;
