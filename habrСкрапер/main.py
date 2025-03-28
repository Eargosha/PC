import requests
from bs4 import BeautifulSoup
import pandas as pd
from concurrent.futures import ThreadPoolExecutor, as_completed # Для конкурентой парарельности
import time
import random

BASE_DELAY = 1.5  # Базовая задержка между запросами к странице со статьями (в секундах)
SMAL_DELAY = 0.5  # Маленкая задержка между запросами к статье (в секундах)
RANDOM_DELAY = 1.0  # Случайная вариация задержки (±X секунд)

def fetch_page_content(url):
    """
    Функция для получения HTML-кода страницы.
    Args:
        url (str): URL страницы для запроса.
    Returns:
        str: HTML-код страницы или None, если запрос не удался.
    """
    try:
        time.sleep(BASE_DELAY + random.uniform(0, RANDOM_DELAY))
      # Мы типо под лисой заходим
        headers = {'User-Agent': 'Mozilla/5.0'}
        response = requests.get(url, headers=headers, timeout=10)
        response.raise_for_status()
        return response.text
    except requests.RequestException as e:
        print(f"Ошибка при запросе к {url}: {e}")
        return None

def parse_news_list(html):
    """
    Функция для парсинга списка новостей на странице.
    Args:
        html (str): HTML-код страницы.
    Returns:
        list: Список словарей с данными о новостях (заголовок, ссылка).
    """
    soup = BeautifulSoup(html, 'html.parser')
    news_list = []


    # Парсим сами плитки новостей
    articles = soup.find_all('article', class_='tm-articles-list__item')
    for article in articles:
        title_tag = article.find('a', class_='tm-title__link')
        if title_tag:
          # По пути получаем название новости и ссылку на нее
            title = title_tag.text.strip()
            link = "https://habr.com" + title_tag['href']
            news_list.append({'title': title, 'link': link})

    return news_list

def parse_article_details(article_url):
    """
    Функция для парсинга деталей новости (дата, теги, текст).
    Args:
        article_url (str): URL статьи.
    Returns:
        dict: Словарь с данными о новости (дата, теги, текст).
    """
    time.sleep(SMAL_DELAY + random.uniform(0, 0.3))
    html = fetch_page_content(article_url)
    if not html:
        return {}

    soup = BeautifulSoup(html, 'html.parser')

    # Находим время
    date_tag = soup.find('time', datetime=True)
    date = date_tag['datetime'] if date_tag else "N/A"

    # Находим теги новости
    tags = [tag.text.strip() for tag in soup.find_all('a', class_='tm-publication-hub__link')]

    # Находим текст новости
    content_tag = soup.find('div', class_='article-formatted-body')
    text = content_tag.text.strip() if content_tag else "N/A"

    return {'date': date, 'tags': tags, 'text': text}

def process_page(base_url, page):
    """
    Обрабатывает одну страницу и возвращает список новостей.
    """
    url = f"{base_url}page{page}/"
    print(f"Обработка страницы {page}...")
    html = fetch_page_content(url)

    if not html:
        return []

    # Получаем сами статьи
    news_list = parse_news_list(html)

    # Параллельная обработка статей, для каждой страницы будет 5 "потоков"
    # Создаётся контекстный менеджер с пулом из 5 рабочих потоков
    with ThreadPoolExecutor(max_workers=5) as executor:
      # Для каждой новости в news_list создаётся задача (future) на выполнение функции parse_article_details с аргументом news['link']
        futures = [executor.submit(parse_article_details, news['link']) for news in news_list]
        # executor.submit() не блокирует выполнение, а сразу возвращает объект Future
        # В результате получаем список futures - обещаний результатов
        # as_completed(futures) возвращает итератор, который yields futures по мере их завершения
        for i, future in enumerate(as_completed(futures)):
          # получает фактический результат выполнения функции (в данном случае словарь с деталями статьи)
            details = future.result()
          # получает фактический результат выполнения функции (в данном случае словарь с деталями статьи)
            news_list[i].update(details)
            time.sleep(SMAL_DELAY +  random.uniform(0, 1)) # Поспим рандомно еще тут

    return news_list

def scrape_habr_pages_parallel(base_url, num_pages):
    """
    Параллельная функция для скрапинга нескольких страниц Хабра.
    """
    all_news = []

    # Создаем задачи для каждой страницы
    with ThreadPoolExecutor(max_workers=3) as executor:
      # Создание задач для каждой страницы
      # executor.submit(process_page, base_url, page) ставит функцию process_page в очередь на выполнение в одном из свободных потоков
        futures = [executor.submit(process_page, base_url, page) for page in range(1, num_pages + 1)]

        # Обработка результатов по мере готовности
        for future in as_completed(futures):
            try:
                time.sleep(BASE_DELAY)
              # блокирует выполнение, пока задача не завершится, и возвращает результат
                page_news = future.result()
                all_news.extend(page_news)
            except Exception as e:
                print(f"Ошибка при обработке страницы: {e}")

    return pd.DataFrame(all_news)

if __name__ == "__main__":
    BASE_URL = "https://habr.com/ru/articles/"
    NUM_PAGES = 10

    start_time = time.time()

    # Скрапинг данных с параллельной обработкой
    news_data = scrape_habr_pages_parallel(BASE_URL, NUM_PAGES)

    # Сохранение данных
    news_data.to_csv("habr_news_parallel.csv", index=False, encoding='utf-8')

    print(f"Данные успешно сохранены. Время выполнения: {time.time() - start_time:.2f} секунд")
