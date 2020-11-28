import React, { useState, useEffect } from 'react'
import ReactSelect from 'react-select'

const PersonSelect = function (props) {
  const { options, name, value, onChange, disabled, placeholder } = props
  const [ list, setList ] = useState(null)
  const [ selected, setSelected] = useState([])
  
  useEffect(function () {
    if (options) {
      setList(options.map(x => ({
        value: x.id,
        label: x.name
      })))
    }
    
  }, [options])
  
  useEffect(function () {
    if (Array.isArray(value)) {
      setSelected(value.map(x => ({
        value: x.id,
        label: x.name
      })))
    }
  }, [value])

  const customStyles = {
    container: (provided) => ({
      ...provided,
      width: '100%'
    })
  }

  const handleChange = function (data) {
    const ids = (data || []).map(x => x.value)
    onChange(options.filter(x => ids.includes(x.id)), name)
  }

  return <>
    <ReactSelect
      styles={customStyles}
      value={selected}
      options={list}
      onChange={handleChange}
      isDisabled={disabled || !options}
      placeholder={!options ? 'Загрузка...' : placeholder}
      isMulti
      components={{
        IndicatorSeparator: null,
        ClearIndicator: null
      }}
    />
  </>
}

export default PersonSelect