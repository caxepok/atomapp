import React, { useState, useEffect } from 'react'
import { useToasts } from 'react-toast-notifications'
import Modal from '../Modal'
import {
  Button,
  Control,
  Textarea,
  Textbox,
  DatePicker,
  Select, PersonSelect,
} from '../../shared'
import RecordButton from './RecordButton'
import {
  fetchTasks,
  fetchTaskWorkers,
  saveTask,
  uploadVoice,
} from '../../services/task'
import './task-modal.scss'
import { shallowEqual, useDispatch, useSelector } from 'react-redux'
import { setTask, setTasks } from '../../reducers/data'
import { useRouteMatch } from 'react-router'
import { setRedirect } from '../../reducers/layout'

const helpText = `
Примеры голосового управления задачами

Пример 1:
«Поставить задачу всем начальникам участка ознакомиться с приказом. Срок исполнения 29 ноября, приоритет высокий. Описание: нам поступил приказ, его нужно донести до всех сотрудников вашего подразделения. Проконтролировать особо людей старше 50 лет.»

Пример 2:
«Поставить задачу всем начальникам участка измерить расход реагентов. Срок исполнения 5 декабря. Приоритет низкий. Описание: С целью учёта материалов необходимо проверить наличие и количестве реагентов для полимеризации плутония на своих участках. Если их не хватит на половину следующего года - доложить.»

Пример 3:
«Поставить задачу заменить комплектацию насосного оборудования. Срок исполнения 2 декабря. Описание: С целью обновления парка насосного оборудования провести замену обмоток тяговых насосов в цехе литья алюминия. Обмотки можно выписать в отделе снабжения.»

Пример 4:
«Поставить задачу всем начальникам участка поставить новогоднюю ёлку. Срок исполнения 31 декабря. Приоритет высокий. Описание: нарядить ее и положить под елку конфеты»

Пример 5:
«Закрыть задачу 18 с коментарием: Задвижки закрыты, давление 60 атмосфер.»

Пример 6:
«Закрыть задачу номер 26»

Пример 7:
«Добавить к задаче номер 34 коментарий: Проконсультируйте на счёт освещения в пятом блоке. Оно тусклое.»
`

const TaskModal = function () {
  const [ isRecognizing, setRecognizing ] = useState(false)
  const [ isSending, setSending ] = useState(false)
  const [ workers, setWorkers ] = useState(null)
  const data = useSelector(({ data }) => data.task, shallowEqual)
  const userId = useSelector(({ user }) => user.id)
  const dispatch = useDispatch()
  const { addToast } = useToasts()
  const match = useRouteMatch({ path: '/:page/*'})

  useEffect(function () {
    setRecognizing(false)
  }, [data])
  
  useEffect(function () {
    if (userId) {
      async function fetchData() {
        try {
          const data = await fetchTaskWorkers(userId)
          setWorkers(data)
        } catch {
          
        }
      }

      fetchData()
    }
    
    return function () {
      dispatch(setTask())
    }
  }, [userId, dispatch])
  
  const handleSendVoice = async function (audioBlob) {
    setRecognizing(true)
    const data = await uploadVoice(userId, audioBlob)

    if (!data.error) {
      dispatch(setTask(data))
    } else {
      addToast(data.error, {
        appearance: 'error',
        autoDismissTimeout: 3000,
        autoDismiss: true
      })
    }

    setRecognizing(false)
  }
  
  const handleChange = function (e, name) {
    if (name) {
      dispatch(setTask({
        ...data,
        [name]: e
      }))
    } else {
      const el = e.currentTarget
      const elName = el.getAttribute('name')

      dispatch(setTask({
        ...data,
        [elName]: el.value
      }))
    }
  }
  
  const handleSaveTask = async function () {
    setSending(true)

    try {
      await saveTask(userId, data)
      const page = match && match.params.page
      const tasks = await fetchTasks(userId, page)
      setSending(false)
      dispatch(setTasks(page, tasks))

      setTimeout(() => dispatch(setRedirect('/outbox')), 1)
    } catch {
      addToast('Не удалось сохранить', {
        appearance: 'error',
        autoDismissTimeout: 3000,
        autoDismiss: true
      })

      setSending(false)
    }
  }
  
  return <Modal 
    title={data.id ? 'Редактировать задачу' : 'Новая задача'} 
    buttons={<Button 
      onClick={handleSaveTask} 
      disabled={
        isRecognizing || 
        isSending || 
        !data.executorWorkers || data.executorWorkers.length === 0
      }>
      Сохранить
    </Button>}
  >
    <div className='task-modal'>
      <div className='form'>
        <Control label='Название'>
          <Textbox
            value={data.name || ''}
            name='name'
            onChange={handleChange}
            disabled={isRecognizing || isSending}
          />
        </Control>
        <Control label='Объект'>
          <Textbox
            value={data.taskObject || ''}
            name='taskObject'
            onChange={handleChange}
            disabled={isRecognizing || isSending}
          />
        </Control>
        <Control label='Дата выполнения'>
          <DatePicker
            value={data.plannedAt || ''}
            name='plannedAt'
            onChange={handleChange}
            disabled={isRecognizing || isSending}
          />
          <Control inline label='Приоритет'>
            <Select
              value={data.priority || ''}
              name='priority'
              onChange={handleChange}
              disabled={isRecognizing || isSending}
              placeholder='Приоритет'
              style={{
                minWidth: 130
              }}
              options={[
                { value: 0, label: 'Низкий'},
                { value: 1, label: 'Средний'},
                { value: 2, label: 'Высокий'},
              ]}
            />
          </Control>
        </Control>
        <Control label='Исполнители'>
          <PersonSelect 
            value={data.executorWorkers}
            name={'executorWorkers'}
            onChange={handleChange}
            disabled={isRecognizing || isSending}
            options={workers}
            placeholder={'Выберите исполнителей'}
          />
        </Control>
        <Control label='Описание'>
          <Textarea 
            rows={7}
            value={data.description || ''}
            name='description'
            onChange={handleChange}
            disabled={isRecognizing || isSending}
          />
        </Control>
        
      </div>
      <RecordButton 
        onSend={handleSendVoice} 
        isDisabled={isRecognizing || isSending}
        isRecognizing={isRecognizing}
      />
      <div className='task-modal-help'>
        <Button small light onClick={() => alert(helpText)}>
          Помощь...
        </Button>
      </div>
    </div>
  </Modal>
}

export default TaskModal