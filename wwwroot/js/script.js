'use strict'

const uri = 'api/prescriptionitems';
let todos = [];


let items = document.querySelectorAll('.item')
const closeBtn = document.querySelector('.closeBtn')
const newTodo = document.querySelector('.newTodo')
const newTodoContainer = document.querySelector('.newTodoContainer')
const todoData = document.querySelector('#todoData')
const submitForm = document.querySelector('.submitForm')
const todoList = document.querySelector('.todoList')

let isEditTodo = false;
let currentEditedID = null;

newTodo.addEventListener('click', () => {
    newTodoContainer.classList.add('show')
    newTodoContainer.querySelector('.title').innerText = 'Adj hozzá új vényt'
})

closeBtn.addEventListener('click', () => {
    newTodoContainer.classList.remove('show')
    todoData.value = ''
})

// GET THEM
function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => showTodo(data))
        .catch(error => console.error('Nem sikerült megtalálni az elemeket', error));
    console.log("sikeres fetch")
}
getItems();

function showTodo(data) {

    let LI = ''
    data.forEach((todo, index) => {
        LI += `<div class="item">
        <div class="header">
            <span class="sNo">${index + 1}</span>
                <p class="todoTitle">${todo.name}</p>
        </div>
            <div class="body buttons">
                <div class="edit" onclick='editItem(${index}) 'data-editID="${todo.id}" data-editData="${todo.name}"><i class="fa-solid fa-pencil"></i></div>
                <div class="delete" onclick='deleteItem(${todo.id})'><i class="fa-solid fa-trash"></i> </div>
            </div>
        </div>`
    })
    todoList.innerHTML = LI || `<p class="noTodo">Nincs felírásra váró gyógyszer! 🥳</p>`

    todos = data;
}


// ADD ITEM
function addItem() {
    const addNameTextbox = todoData.value.trim();

    const item = {
        isPrescribed: false,
        name: addNameTextbox
    };
    console.log(item)

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
        })
        .catch(error => console.error('Nem sikerült hozzáadni az elemet.', error));

    todoData.value = '';
    newTodoContainer.classList.remove('show');
}


// DELETE ITEM
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Nem sikerült törölni az elemet.', error));
}


// EDIT ITEM
function editItem(id) {
    newTodoContainer.classList.add('show')
    newTodoContainer.querySelector('.title').innerText = 'Módosítás'


    const edit = document.querySelectorAll('.edit')

    todoData.value = edit[id].dataset.editdata;
    currentEditedID = edit[id].dataset.editid;
    isEditTodo = true;
}

// SUBMIT BUTTON
submitForm.addEventListener('click', (e) => {
    e.preventDefault();

    if (!isEditTodo) {
        addItem();
    } else {
        console.log(todos);
        console.log(currentEditedID)

        const item = {
            id: parseInt(currentEditedID, 10),
            isComplete: true,
            name: todoData.value.trim()
        }

        fetch(`${uri}/${currentEditedID}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(() => getItems())
            .catch(error => console.error('Nem sikerult modositani az elemet.', error));
        isEditTodo = false;
    }
    todoData.value = '';
    newTodoContainer.classList.remove('show');
});