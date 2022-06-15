# gmcstonedesafio

#Como executar

Este exercício foi desenvolvido em C# versão .Net 5.0 
Para compilar e executar tem que ter instalado .NET Runtime 5.0 (ou maior)
(https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

Para executar, após feito o dowload do respositório 
basta alterar o diretorio para /DesafioFaturamento
que contem o arquivo DesafioFaturamento.csproj

E executar o comando
>dotnet run [drive:][path]nomeDoArquivo

Parametros entre colchetes são opcionais


Uma pequena funcionalidade que utilizei na construção para me ajudar nos testes, 
é rodar o comando da forma
>dotnet run true

Assim o programa utiliza como arquivo de entrada o csv encontrado na pasta 
DesafioFaturamento/data, ele contém os mesmos dados de coleta informados na 
especificação, e valida com a saída esperada.


# Decisões ao realizar e documentação

.Net se baseia na linguagem do sistema operacional para tratar string como 
numero decimais ('.' para casas decimais, e ',' para separador de milhar, e 
vice-versa) então usei uma configuracao global para sempre tratar '.' para casas 
decimais, indepentende do SO, metodos para esse tratamento estao na classe 
FaturamentoUtils

.Net possui alguns métodos diferentes para realizar o arredondamento de valores, 
eu utilizei o MidpointRounding.AwayFromZero, ou seja, vai arredondar para o 
numero mais perto, quando esta no meio do caminho entre dois numeros arredonda 
para o número mais distante de zero.
Ex 3,234 arredonda para 3,23; 3,235 arredonda para 3,24; 3,236 arredonda para 
3,24.

O arredondamento é realizado na leitura dos valores, pelo que entendi que era a 
especificação dos requisitos "Todos os preços terão no máximo duas casas 
decimais, arredondados para a casa decimal mais próxima" e não somente após as 
totalizações serem realizadas.


Linhas que falharam na validação são ignoradas para o totalizar o valor da nota, 
com exceção dos casos em que a validação depende da combinação de linhas, e não 
teria uma maneira segura de garantir a precedência, nesse caso todas as entradas 
da maquina são ignoradas.

Estes casos são:
- Não podem existir duas datas iniciais iguais para um mesmo id.
- entre o período de suas datas.
- Uma maquininha só poderá ter um evento de Desativação após uma
Ativação.

Validações tanto dos comandos de entrada, das linhas e da consistência dos 
eventos para uma maquina são realizadas na classe FaturamentoValidator e todo o 
calculo e regras de negócio associadas na classe FaturamentoCalculator.

Não foi realizada a atividade bônus.