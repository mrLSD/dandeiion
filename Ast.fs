module dandeiion.Ast

type Ident = {
    span: string
}
    
type ConstantName = {
        ident: Ident
    } with
    member this.getName =
        this.ident.span

type FunctionName = {
        ident: Ident
    } with
    member this.getName =
        this.ident.span

type ParameterName = {
    ident: Ident
}

type ValueName = {
        ident: Ident
    } with
    member this.getName =
        this.ident.span


type ImportName = {
        ident: Ident
    } with
    member this.getName =
        this.ident.span

type ImportPath = ImportName[]

type PrimitiveTypes =
    | U8 
    | U16 
    | U32 
    | U64
    | I8
    | I16
    | I32
    | I64
    | F32
    | F64
    | Bool
    | Char
    | String
    | None
    member this.getName =
        match this with
        | U8 -> "u8"
        | U16 -> "u16"
        | U32 -> "u32"
        | U64 -> "u64"
        | I8 -> "i8"
        | I16 -> "i16"
        | I32 -> "i32"
        | I64 -> "i64"
        | F32 -> "f32"
        | F64 -> "f64"
        | Bool -> "bool"
        | Char -> "char"
        | String -> "string"
        | None -> "()"

type Type =
    | Primitive of PrimitiveTypes
    | Struct of StructTypes
    | Array of Type * uint32
    member this.getName =
        ""

and StructType = {
    attrName: Ident
    attrType: Type
}

and StructTypes =
    {
        name: Ident
        types: StructType[]
    }
    member this.getName =
        this.name.span
    

type PrimitiveValue =
    | U8 of uint8
    | U16 of uint16
    | U32 of uint32
    | U64 of uint64
    | I8 of int8
    | I16 of int16
    | I32 of int32
    | I64 of int64
    | F32 of float32
    | F64 of double
    | Bool of bool
    | String of string
    | Char of char
    | None


type ConstantValue =
    | Constant of ConstantName
    | Value of PrimitiveValue

type ExpressionOperations =
    | Plus
    | Minus
    | Multiply
    | Divide
    | ShiftLeft
    | ShiftRight
    | And
    | Or
    | Xor
    | Eq
    | NotEq
    | Great
    | Less
    | GreatEq
    | LessEq

type ExpressionValue =
    | ValueName of ValueName
    | PrimitiveValue of PrimitiveValue
    | FunctionCall of FunctionCall

and FunctionCall = {
        name: FunctionName
        parameters: Expression[]    
    } with
    member this.getName =
        this.name.getName

and Expression = {
    expression_value: ExpressionValue
    operation: (ExpressionOperations * Expression) option
}

type ConstantExpression = {
    value: ConstantValue
    operation: (ExpressionOperations * ConstantExpression) option
}

type Constant = {
        name: ConstantName 
        constant_type: Type
        constant_value: ConstantExpression
    } with
    member this.getName =
        this.name.getName   

type FunctionParameter = {
    name: ParameterName
    parameterType: Type
}

type LetBinding = {
    name: ValueName
    value_type: Type option
    value: Expression
}

type Condition =
    | Great
    | Less
    | Eq
    | GreatEq
    | LessEq

type LogicCondition = 
    | And
    | Or

type ExpressionCondition = {
    left: Expression
    condition: Condition
    right: Expression
}

type ExpressionLogicCondition = {
    left: ExpressionCondition
    right: (LogicCondition * ExpressionLogicCondition) option
}

type IfCondition =
    | Single of Expression
    | Logic of ExpressionLogicCondition

type IfStatement = {
    condition: IfCondition
    body: IfBodyStatement
    else_statement: IfBodyStatement[] option
    else_if_statement: IfStatement[] option
}

and IfBodyStatement =
    | LetBinding of LetBinding
    | FunctionCall of FunctionCall
    | If of IfStatement
    | Loop of LoopBodyStatement[]
    | Return of Expression

and LoopBodyStatement =
    | LetBinding of LetBinding
    | FunctionCall of FunctionCall
    | IfLoop of IfLoopStatement
    | Loop of LoopBodyStatement[]
    | Return of Expression
    | Break
    | Continue

and IfLoopStatement = {
    condition: IfCondition
    body: IfLoopBodyStatement[]
    else_statement: IfLoopBodyStatement[] option
    else_if_statement: IfLoopStatement[] option
}

and IfLoopBodyStatement =
    | LetBinding of LetBinding
    | FunctionCall of FunctionCall
    | IfLoop of IfLoopStatement
    | Loop of LoopBodyStatement[]
    | Return of Expression
    | Break
    | Continue

type BodyStatement =
    | LetBinding of LetBinding
    | FunctionCall of FunctionCall
    | If of IfStatement
    | Loop of LoopBodyStatement[]
    | Expression of Expression
    | Return of Expression
    
type FunctionStatement = {
        name: FunctionName
        parameters: FunctionParameter[]
        resultType: Type
        body: BodyStatement[]
    } with
    member this.getName =
        this.name.getName

type MainStatement =
    | Import of ImportPath
    | Constant of Constant
    | Types of StructTypes
    | Function of FunctionStatement
