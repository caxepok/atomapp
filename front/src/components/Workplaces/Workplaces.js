import React from 'react'
import { useSelector, shallowEqual, useDispatch } from 'react-redux'
import LayoutSection from '../../components/Layout/LayoutSection'
import Table from '../../components/Table'
import { setUser } from '../../reducers/user'
import { Button, Loader } from '../../shared'
import { ROLES } from '../../shared/consts'
import { PageLoader, PageMessage } from '../Page'

const Workplaces = function () {
  const users = useSelector(({ data }) => data.users, shallowEqual)
  const dispatch = useDispatch()
  
  const handleClick = function (user) {
    dispatch(setUser(user))
  }
  
  if (users === undefined) {
    return <LayoutSection>
      <PageMessage>
        Выберите подразделение
      </PageMessage>
    </LayoutSection>
  }
  
  return <LayoutSection
    header={users ? <span>{users.name}</span> : users === null ? <Loader /> : ''}
  >
    {users
      ? users.list && users.list.length 
        ? <Table
          sizes={[
            '50px',
            '50%',
            '50%',
            '100px'
          ]}
          data={users && users.list.map(user => ({
            datum: {
              id: user.id,
              name: user.name,
              role: ROLES[user.role],
              button: <Button to='/inbox' small light
                              onClick={() => handleClick(user)}>
                Залогиниться
              </Button>
            }
          }))}
        />
        : <PageMessage>
          Нет пользователей
        </PageMessage>
      : users === null ? <PageLoader/> : null
    }
  </LayoutSection>
}

export default Workplaces