import requests
from bs4 import BeautifulSoup
import pandas as pd

def fetch_page_content(url):
    """
    Функция для получения HTML-кода страницы.
    
    Args:
        url (str): URL страницы для запроса.
    
    Returns:
        str: HTML-код страницы или None, если запрос не удался.
    """
    try:
        response = requests.get(url)
        response.raise_for_status()  # Проверка на ошибки HTTP
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
    
    # Поиск всех карточек новостей
    articles = soup.find_all('article', class_='tm-articles-list__item')
    for article in articles:
        title_tag = article.find('a', class_='tm-title__link')
        if title_tag:
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
    html = fetch_page_content(article_url)
    if not html:
        return {}
    
    soup = BeautifulSoup(html, 'html.parser')
    
    # Извлечение даты публикации
    date_tag = soup.find('time', datetime=True)
    date = date_tag['datetime'] if date_tag else "N/A"
    
    # Извлечение тегов
    tags = [tag.text.strip() for tag in soup.find_all('a', class_='tm-publication-hub__link')]
    
    # Извлечение текста новости
    content_tag = soup.find('div', class_='article-formatted-body')
    text = content_tag.text.strip() if content_tag else "N/A"
    
    return {'date': date, 'tags': tags, 'text': text}


def scrape_habr_pages(base_url, num_pages):
    """
    Функция для скрапинга нескольких страниц Хабра.
    
    Args:
        base_url (str): Базовый URL для скрапинга.
        num_pages (int): Количество страниц для обработки.
    
    Returns:
        pd.DataFrame: DataFrame с данными о новостях.
    """
    all_news = []
    
    for page in range(1, num_pages + 1):
        print(f"Обработка страницы {page}...")
        url = f"{base_url}page{page}/"
        html = fetch_page_content(url)
        
        if html:
            news_list = parse_news_list(html)
            for news in news_list:
                details = parse_article_details(news['link'])
                news.update(details)  # Добавляем детали новости
                all_news.append(news)
    
    # Создание DataFrame
    df = pd.DataFrame(all_news)
    return df


if __name__ == "__main__":
    # Базовый URL для скрапинга
    BASE_URL = "https://habr.com/ru/articles/"
    
    # Количество страниц для обработки
    NUM_PAGES = 1
    
    # Скрапинг данных
    news_data = scrape_habr_pages(BASE_URL, NUM_PAGES)
    
    # Сохранение данных в CSV файл
    news_data.to_csv("habr_news.csv", index=False, encoding='utf-8')
    print("Данные успешно сохранены в файл habr_news.csv")
