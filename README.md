# ProcessamentoFila

Esse projeto possui 2 métodos. o Post grava informações no kafka. O Get recupera um item da fila

o yaml abaixo é para a configuração no docker. 

Execute o comando abaixo no cmd para configurar: docker-compose -f compose.yaml up


version: '2'
services:
  zookeeper:
    image: confluentinc/cp-zookeeper:latest
    networks: 
      - net
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  kafka:
    image: confluentinc/cp-kafka:latest
    networks: 
      - net
    depends_on:
      - zookeeper
    ports:
      - 9092:9092
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1

networks: 
  net:
    driver: bridge
    
    
