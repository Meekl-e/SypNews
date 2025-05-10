import requests



def ask(text):
    prompt = {
        "modelUri": "gpt://b1g2kgtu7de8nnc06bki/yandexgpt-lite",
        "completionOptions": {
            "stream": False,
            "temperature": 0.6,
            "maxTokens": "2000"
        },
        "messages": [
            {
                "role": "system",
                "text": "Ты ассистент, который ищет и создает интересные факты о днях с привязкой к городу. Примерно 30 слов."
            },
            {
                "role": "user",
                "text": text
            },
        ]
    }

    url = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion"
    headers = {
        "Content-Type": "application/json",
        "Authorization": "Api-Key AQVN0fEqdKE98QzMz94LWn9uoGsek2GzJi0A5WiZ"
    }

    response = requests.post(url, headers=headers, json=prompt)
    result = response.json()
    return result["result"]["alternatives"][0]["message"]["text"]
#http://127.0.0.1:8000/ssyp/newspaper?date=11.09.2006
