import React, { useState, useEffect } from 'react'
import ReactSelect from 'react-select'

const Select = function (props) {
  const { options, name, value, onChange, style, disabled, placeholder } = props
  const [ selected, setSelected ] = useState()
  
  const customStyles = {
    control: (provided) => ({
      ...provided,
      ...style
    })
  }
  
  useEffect(function () {
    setSelected(options.find(x => x.value === value))
  }, [options, value])
  
  
  const handleChange = function (option) {
    onChange(option.value, name)
  }

  return <>
    <ReactSelect 
      styles={customStyles} 
      value={selected} 
      options={options} 
      onChange={handleChange}
      isDisabled={disabled}
      placeholder={placeholder}
      components={{
        IndicatorSeparator: null
      }}
    />
  </>
}

export default Select