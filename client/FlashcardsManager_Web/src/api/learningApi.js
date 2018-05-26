import { getApiCall, postApiCall } from './index';
import { apiUrls} from './apiUrls';

const mockCategories = [
    {
        "id": 1,
        "name": "Anatomia"
    },
    {
        "id": 2,
        "name": "Fiszki polsko-angielskie"
    },
    {
        "id": 3,
        "name": "Anatomia2"
    }
];

const mockFlashcards = [
    {
        "id": 1,
        "key": "bezzałogowy",
        "value": "unmanned",
        "keyDescription": "Statek kosmiczny, który eksplodował, na szczęście był bezzałogowy",
        "valueDescription": "Fortunately, the spacecraft that exploded was unmanned",
        "categoryId": 2,
        "category": null
    },
    {
        "id": 3,
        "key": "różdżka",
        "value": "wand",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 4,
        "key": "pióro",
        "value": "quill",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 5,
        "key": "ogier",
        "value": "stallion",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 6,
        "key": "kastrować",
        "value": "geld",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 7,
        "key": "żywopłot",
        "value": "hedge",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 14,
        "key": "bochenek",
        "value": "loaf",
        "keyDescription": null,
        "valueDescription": null,
        "categoryId": 2,
        "category": null
    },
    {
        "id": 15,
        "key": "odmówić",
        "value": "refuse",
        "keyDescription": "Złożę mu taką ofertę, że nie będzie mógł odmówić.",
        "valueDescription": "I'll make him offer, he cannot refuse.",
        "categoryId": 2,
        "category": null
    },
    {
        "id": 16,
        "key": "oczekujący",
        "value": "pending",
        "keyDescription": null,
        "valueDescription": "You've got 2 pending requests",
        "categoryId": 2,
        "category": null
    },
    {
        "id": 17,
        "key": "niezliczony",
        "value": "innumerable",
        "keyDescription": "Liczba prób potrzebna, by to zaczęło działać, była niezliczona",
        "valueDescription": "I've tried to make it display properly innumerable times",
        "categoryId": 2,
        "category": null
    },
];


export function getCategoriesApiCall(bearerToken) {
    const url = apiUrls.categoriesEndpoint;
    //return new Promise((resolve, reject) => resolve(mockCategories));
    return getApiCall(url, bearerToken)
        .then(json => json.map(arrayMember => {
            return {
                id: arrayMember.id,
                name: arrayMember.name,
            };
        })
        );
}

export function getLearningFlashcardsApiCall(categoryId, mode, bearerToken) {
    const url = apiUrls.learningFlashcardsEndpoint + `/${categoryId}?strategy=${mode}`;
    //return new Promise((resolve, reject) => resolve(mockFlashcards));
        return getApiCall(url, bearerToken)
        .catch(error => Promise.reject())
        .then(json => json.map(arrayMember => {
            return {
                id: arrayMember.id,
                key: arrayMember.key,
                value: arrayMember.value,
                keyDescription: arrayMember.keyDescription,
                valueDescription: arrayMember.valueDescription
            };
        })
       );
}

export function proceedFlashcardApiCall(flashcardId, result, bearerToken) {
    //return new Promise((resolve, reject) => resolve());
    const url = apiUrls.learningResultEndpoint;
    const data = {
       flashcardId,
       result
    };
    return postApiCall(url, data, bearerToken);
}

