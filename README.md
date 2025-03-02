# Проект MovieCatalog

Этот проект состоит из консольного приложения и библиотечного проекта.

## Необходимые условия

- Установленный Docker

## Запуск проекта

1. Склонируйте репозиторий:

   ```bash
   git clone https://github.com/ваш-репозиторий/MovieCatalog.git

2. Перейдите в директорию проекта:

   ```bash
   cd MovieCatalog

3. Соберите Docker-образ:
   ```bash
   docker build -t moviecatalog .
   
4. Запустите контейнер:
   ```bash
   docker run -it --rm moviecatalog