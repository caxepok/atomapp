import React, { useEffect } from 'react'
import LayoutSection from '../../components/Layout/LayoutSection'
import { useRouteMatch } from 'react-router-dom'
import { shallowEqual, useDispatch, useSelector } from 'react-redux'
import { PageLoader, PageMessage } from '../../components/Page'
import { loadTasks, loadWorkers } from '../../reducers/data'
import Table from '../../components/Table'
import { createPathURL, getFormattedDate } from '../../shared/helpers'
import { DatumPriority, DatumStatus } from '../../components/Datum'
import TaskDetails from '../../components/TaskDetails'

const Inbox = function () {
  const match = useRouteMatch()
  const data = useSelector(({ data }) => data.inbox, shallowEqual)
  const workers = useSelector(({ data }) => data.workers, shallowEqual)
  const dispatch = useDispatch()
  const taskId = match.params.task

  useEffect(function () {
    dispatch(loadTasks('inbox'))
    dispatch(loadWorkers())
  }, [dispatch])

  return <>
    <LayoutSection header={(
      <strong>Мои задачи</strong>
    )}>
      {!data || !workers
        ? <PageLoader />
        : data.length > 0
          ? <Table
            labels={[
              <strong style={{fontSize: '14px'}}>⥮</strong>,
              '№',
              'Название',
              'Объект',
              'Выполнить до',
              'Постановщик',
              'Статус'
            ]}
            sizes={[
              '30px',
              '50px',
              'auto',
              'auto',
              '150px',
              '150px',
              '110px',
            ]}
            data={data.map(item => ({
              to: createPathURL(match.path, {...match.params, task: item.id}),
              selected: match.params.task === item.id.toString(),
              datum: {
                priority: <DatumPriority priority={item.priority} />,
                id: item.id,
                name: <strong>{item.name}</strong>,
                taskObject: item.taskObject,
                plannedAt: getFormattedDate(item.plannedAt),
                creator: workers.find(x => x.id === item.creatorId).name,
                status: item.isFinished
                  ? <DatumStatus className={'-success'}>Завершено</DatumStatus>
                  : <DatumStatus>Активно</DatumStatus>
              }
            }))}
          />
        : <PageMessage>
          Задачи отсутствуют
        </PageMessage>
      }
    </LayoutSection>
    <LayoutSection aside header={taskId ? `Задача №${taskId}` : ''}>
      <TaskDetails />
    </LayoutSection>
  </>
}

export default Inbox