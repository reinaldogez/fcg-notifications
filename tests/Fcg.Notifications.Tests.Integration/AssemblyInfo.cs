// Coleções de integração sobem containers e o host real; rodá-las em paralelo gera disputa de
// recursos (brokers concorrentes, variável de ambiente compartilhada pelo host do Api) e deixa os
// testes flaky. Serializa-se a suíte — dentro de cada coleção já era sequencial via fixture única.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
