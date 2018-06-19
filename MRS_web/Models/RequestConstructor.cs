using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MRS_web.Models;
using MRS_web.Models.EDM;
using TimeSpan = MRS_web.Models.EDM.TimeSpan;
using Type = MRS_web.Models.EDM.Type;

namespace MRS_web.Models
{
    class RequestConstructor
    {
        /*          СЧётчик
                
                    string = Название
                    string = Описание
                    Float = Сумма показаний
                    Float = Размерность табло
                    Integer = Заводской номер
                    date = Дата производства
                    coll = Параметры
                    entity" name="Tariff = Тариф        //TODO
                    entity" name="string = Тип          //TODO
                    coll = Документы
                    entity" name="User = Пользователь   //TODO
                    coll = Показания
                
            
            
                Уст.Счётчик
                
                    date = Дата установки
                    date = Дата проверки
                    coll = Врем. промежутки
                
            
            
                Пользователь
                
                    string = Логин
                    string = Полное Имя
                    bool = Администратор?
                    coll = Счётчики
                
            
            
                Тип
                
                    string = Название
                    string = Ед. измерения
                    coll = Счётчики
                
            
            
                Показатель
                
                    Float = Значение
                    Integer = Номер тарифа
                    entity" name="Meter = Счётчик //TODO
                
            
            
                Параметр
                
                    string = Название
                    string = Значение
                    coll = Счётчики
                
            
            
                Тариф
                
                    string = Название
                    coll = Счётчики
                    coll = Врем. промежутки
                
            
            
                Врем. промежуток
                
                    string = Название
                    time = Время начала
                    time = Время окончания
                    entity" name="Tariff = Тариф //TODO
                
            
            
                Документ
                
                    string = Заголовок
                    string = Описание
                    date = Дата подписания
                    entity" name="Meter = Счётчик //TODO
               
         */
        //TODO ненавижу тип entity

        /*Счётчик*/
         static Dictionary<string, Dictionary<string, Func<Meter, string, bool>>> MeterDict =
            new Dictionary<string, Dictionary<string, Func<Meter, string, bool>>>
            {
                {
                    "Название", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Name == inp},
                        {"!=", (t, inp) => t.Name != inp}
                    }
                },
                {
                    "Описание", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Discription == inp},
                        {"!=", (t, inp) => t.Discription != inp}
                    }
                },
                {
                    "Сумма показаний", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.SumReadings.Equals(double.Parse(inp))},
                        {"!=", (t, inp) => !t.SumReadings.Equals(double.Parse(inp))},
                        {">", (t, inp) => t.SumReadings > double.Parse(inp)},
                        {"<", (t, inp) => t.SumReadings < double.Parse(inp)},
                        {">=", (t, inp) => t.SumReadings >= double.Parse(inp)},
                        {"<=", (t, inp) => t.SumReadings <= double.Parse(inp)}
                    }
                },
                {
                    "Размерность табло", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Capacity.Equals(double.Parse(inp))},
                        {"!=", (t, inp) => !t.Capacity.Equals(double.Parse(inp))},
                        {">", (t, inp) => t.Capacity > double.Parse(inp)},
                        {"<", (t, inp) => t.Capacity < double.Parse(inp)},
                        {">=", (t, inp) => t.Capacity >= double.Parse(inp)},
                        {"<=", (t, inp) => t.Capacity <= double.Parse(inp)}
                    }
                },
                {
                    "Заводской номер", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Capacity.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Capacity.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Capacity > int.Parse(inp)},
                        {"<", (t, inp) => t.Capacity < int.Parse(inp)},
                        {">=", (t, inp) => t.Capacity >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Capacity <= int.Parse(inp)}
                    }
                },
                {
                    "Дата производства", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.ProductionDate.Equals(DateTime.Parse(inp))},
                        {"!=", (t, inp) => !t.ProductionDate.Equals(DateTime.Parse(inp))},
                        {">", (t, inp) => t.ProductionDate > DateTime.Parse(inp)},
                        {"<", (t, inp) => t.ProductionDate < DateTime.Parse(inp)},
                        {">=", (t, inp) => t.ProductionDate >= DateTime.Parse(inp)},
                        {"<=", (t, inp) => t.ProductionDate <= DateTime.Parse(inp)}
                    }
                },
                {
                    "Параметры", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Parametrs.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Parametrs.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Parametrs.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Parametrs.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Parametrs.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Parametrs.Count <= int.Parse(inp)}
                    }
                },
                // inp содержит весь путь до поля сущности без первого Entity и первого поля, пример (в классе Meter):  Name.Value
                /*{
                    "Тариф", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {
                            "==", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Name":
                                        return t.Tariff.Name.Equals(value);
                                    case "TimeSpans":
                                        return t.Tariff.TimeSpans.Count.Equals(int.Parse(value));
                                    case "Meters":
                                        return t.Tariff.Meters.Count.Equals(int.Parse(value));
                                    default: return false;
                                }
                            }
                        },
                        {
                            "!=", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Name":
                                        return !t.Tariff.Name.Equals(value);
                                    case "TimeSpans":
                                        return !t.Tariff.TimeSpans.Count.Equals(int.Parse(value));
                                    case "Meters":
                                        return !t.Tariff.Meters.Count.Equals(int.Parse(value));
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "TimeSpans":
                                        return t.Tariff.TimeSpans.Count > int.Parse(value);
                                    case "Meters":
                                        return t.Tariff.Meters.Count > int.Parse(value);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "TimeSpans":
                                        return t.Tariff.TimeSpans.Count < int.Parse(value);
                                    case "Meters":
                                        return t.Tariff.Meters.Count < int.Parse(value);
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">=", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "TimeSpans":
                                        return t.Tariff.TimeSpans.Count >= int.Parse(value);
                                    case "Meters":
                                        return t.Tariff.Meters.Count >= int.Parse(value);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<=", (t, inp) =>
                            {
                                string value = inp.Substring(inp.IndexOf('.') + 1);
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "TimeSpans":
                                        return t.Tariff.TimeSpans.Count <= int.Parse(value);
                                    case "Meters":
                                        return t.Tariff.Meters.Count <= int.Parse(value);
                                    default: return false;
                                }
                            }
                        }
                    }
                },*/
                // input содержит весь путь до поля сущности без первого Entity, пример (в классе Meter):  Type.Name
                /*{
                    "Тип", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {
                            "==", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Name":
                                        return t.Type.Equals(inp);
                                    case "Unit":
                                        return t.Type.Unit.Equals(int.Parse(inp));
                                    case "Meters":
                                        return t.Type.Meters.Count.Equals(int.Parse(inp));
                                    default: return false;
                                }
                            }
                        },
                        {
                            "!=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Name":
                                        return !t.Type.Equals(inp);
                                    case "Unit":
                                        return !t.Type.Unit.Equals(int.Parse(inp));
                                    case "Meters":
                                        return !t.Type.Meters.Count.Equals(int.Parse(inp));
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.Type.Meters.Count > int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.Type.Meters.Count < int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.Type.Meters.Count >= int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.Type.Meters.Count <= int.Parse(inp);
                                    default: return false;
                                }
                            }
                        }
                    }
                },*/
                {
                    "Документы", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Documents.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Documents.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Documents.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Documents.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Documents.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Documents.Count <= int.Parse(inp)}
                    }
                },
                // input содержит весь путь до поля сущности без первого Entity, пример (в классе Meter):  User.Login
                {
                    "Пользователь", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {
                            "==", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "FullName":
                                        return t.User.FullName.Equals(inp);
                                    case "Unit":
                                        return t.User.Login.Equals(inp);
                                    case "AdminPrivileges":
                                        return t.User.AdminPrivileges.Equals(bool.Parse(inp));
                                    case "Meters":
                                        return t.User.Meters.Count.Equals(int.Parse(inp));
                                    default: return false;
                                }
                            }
                        },
                        {
                            "!=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "FullName":
                                        return !t.User.FullName.Equals(inp);
                                    case "Unit":
                                        return !t.User.Login.Equals(inp);
                                    case "AdminPrivileges":
                                        return !t.User.AdminPrivileges.Equals(bool.Parse(inp));
                                    case "Meters":
                                        return !t.User.Meters.Count.Equals(int.Parse(inp));
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.User.Meters.Count > int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.User.Meters.Count < int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            ">=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.User.Meters.Count >= int.Parse(inp);
                                    default: return false;
                                }
                            }
                        },
                        {
                            "<=", (t, inp) =>
                            {
                                switch (inp.Remove(inp.IndexOf('.')))
                                {
                                    case "Meters":
                                        return t.User.Meters.Count <= int.Parse(inp);
                                    default: return false;
                                }
                            }
                        }
                    }
                },

                /*{
                    "Показания", new Dictionary<string, Func<Meter, string, bool>>
                    {
                        {"==", (t, inp) => t.Readings.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Readings.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Readings.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Readings.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Readings.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Readings.Count <= int.Parse(inp)}
                    }
                }*/
            };

        /*Уст. Счётчик*/
         static Dictionary<string, Dictionary<string, Func<InstalledMeter, string, bool>>> InstMeterDict =
            new Dictionary<string, Dictionary<string, Func<InstalledMeter, string, bool>>>
            {
                {
                    "Дата установки", new Dictionary<string, Func<InstalledMeter, string, bool>>
                    {
                        {"==", (t, inp) => t.InstallDate.Equals(DateTime.Parse(inp))},
                        {"!=", (t, inp) => !t.InstallDate.Equals(DateTime.Parse(inp))},
                        {">", (t, inp) => t.InstallDate > DateTime.Parse(inp)},
                        {"<", (t, inp) => t.InstallDate < DateTime.Parse(inp)},
                        {">=", (t, inp) => t.InstallDate >= DateTime.Parse(inp)},
                        {"<=", (t, inp) => t.InstallDate <= DateTime.Parse(inp)}
                    }
                },
                {
                    "Дата проверки", new Dictionary<string, Func<InstalledMeter, string, bool>>
                    {
                        {"==", (t, inp) => t.ExpirationDate.Equals(DateTime.Parse(inp))},
                        {"!=", (t, inp) => !t.ExpirationDate.Equals(DateTime.Parse(inp))},
                        {">", (t, inp) => t.ExpirationDate > DateTime.Parse(inp)},
                        {"<", (t, inp) => t.ExpirationDate < DateTime.Parse(inp)},
                        {">=", (t, inp) => t.ExpirationDate >= DateTime.Parse(inp)},
                        {"<=", (t, inp) => t.ExpirationDate <= DateTime.Parse(inp)}
                    }
                }
            };

        /*Пользователь*/
         static Dictionary<string, Dictionary<string, Func<User, string, bool>>> UserDict =
            new Dictionary<string, Dictionary<string, Func<User, string, bool>>>
            {
                {
                    "Логин", new Dictionary<string, Func<User, string, bool>>
                    {
                        {"==", (t, inp) => t.Login.Equals(inp)},
                        {"!=", (t, inp) => !t.Login.Equals(inp)},
                    }
                },
                {
                    "Полное Имя", new Dictionary<string, Func<User, string, bool>>
                    {
                        {"==", (t, inp) => t.FullName.Equals(inp)},
                        {"!=", (t, inp) => !t.FullName.Equals(inp)},
                    }
                },
                {
                    "Администратор?", new Dictionary<string, Func<User, string, bool>>
                    {
                        {"==", (t, inp) => t.AdminPrivileges.Equals(bool.Parse(inp))},
                        {"!=", (t, inp) => !t.AdminPrivileges.Equals(bool.Parse(inp))},
                    }
                },
                {
                    "Счётчики", new Dictionary<string, Func<User, string, bool>>
                    {
                        {"==", (t, inp) => t.Meters.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Meters.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Meters.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Meters.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Meters.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Meters.Count <= int.Parse(inp)}
                    }
                },
            };

        /*Тип*/
         static Dictionary<string, Dictionary<string, Func<Type, string, bool>>> TypeDict =
            new Dictionary<string, Dictionary<string, Func<Type, string, bool>>>
            {
                {
                    "Название", new Dictionary<string, Func<Type, string, bool>>
                    {
                        {"==", (t, inp) => t.Name.Equals(inp)},
                        {"!=", (t, inp) => !t.Name.Equals(inp)},
                    }
                },
                {
                    "Ед. измерения", new Dictionary<string, Func<Type, string, bool>>
                    {
                        {"==", (t, inp) => t.Unit.Equals(inp)},
                        {"!=", (t, inp) => !t.Unit.Equals(inp)},
                    }
                },
                {
                    "Счётчики", new Dictionary<string, Func<Type, string, bool>>
                    {
                        {"==", (t, inp) => t.Meters.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Meters.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Meters.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Meters.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Meters.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Meters.Count <= int.Parse(inp)}
                    }
                },
            };

        /*Показатель*/
         static Dictionary<string, Dictionary<string, Func<Reading, string, bool>>> ReadingDict =
            new Dictionary<string, Dictionary<string, Func<Reading, string, bool>>>
            {
                {
                    "Значение", new Dictionary<string, Func<Reading, string, bool>>
                    {
                        {"==", (t, inp) => t.Value.Equals(double.Parse(inp))},
                        {"!=", (t, inp) => !t.Value.Equals(double.Parse(inp))},
                        {">", (t, inp) => t.Value > double.Parse(inp)},
                        {"<", (t, inp) => t.Value < double.Parse(inp)},
                        {">=", (t, inp) => t.Value >= double.Parse(inp)},
                        {"<=", (t, inp) => t.Value <= double.Parse(inp)}
                    }
                },
                {
                    "Номер тарифа", new Dictionary<string, Func<Reading, string, bool>>
                    {
                        {"==", (t, inp) => t.TariffNumber.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.TariffNumber.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.TariffNumber > int.Parse(inp)},
                        {"<", (t, inp) => t.TariffNumber < int.Parse(inp)},
                        {">=", (t, inp) => t.TariffNumber >= int.Parse(inp)},
                        {"<=", (t, inp) => t.TariffNumber <= int.Parse(inp)}
                    }
                },
                /*{
                    "Счётчик", new Dictionary<string, Func<Reading, string, bool>>
                    {
                        {"==", (t, inp) => ,
                        {"!=", (t, inp) => !t.TariffNumber.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.TariffNumber > int.Parse(inp)},
                        {"<", (t, inp) => t.TariffNumber < int.Parse(inp)},
                        {">=", (t, inp) => t.TariffNumber >= int.Parse(inp)},
                        {"<=", (t, inp) => t.TariffNumber <= int.Parse(inp)}
                    }
                },*/
            };

        /*Параметр*/
         static Dictionary<string, Dictionary<string, Func<Parametr, string, bool>>> ParametrDict =
            new Dictionary<string, Dictionary<string, Func<Parametr, string, bool>>>
            {
                {
                    "Название", new Dictionary<string, Func<Parametr, string, bool>>
                    {
                        {"==", (t, inp) => t.Name.Equals(inp)},
                        {"!=", (t, inp) => !t.Name.Equals(inp)},
                    }
                },
                {
                    "Значение", new Dictionary<string, Func<Parametr, string, bool>>
                    {
                        {"==", (t, inp) => t.Measure.Equals(inp)},
                        {"!=", (t, inp) => !t.Measure.Equals(inp)},
                    }
                },
                {
                    "Счётчики", new Dictionary<string, Func<Parametr, string, bool>>
                    {
                        {"==", (t, inp) => t.Meters.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Meters.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Meters.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Meters.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Meters.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Meters.Count <= int.Parse(inp)}
                    }
                },
            };

        /*Тариф*/
         static Dictionary<string, Dictionary<string, Func<Tariff, string, bool>>> TariffDict =
            new Dictionary<string, Dictionary<string, Func<Tariff, string, bool>>>
            {
                {
                    "Название", new Dictionary<string, Func<Tariff, string, bool>>
                    {
                        {"==", (t, inp) => t.Name.Equals(inp)},
                        {"!=", (t, inp) => !t.Name.Equals(inp)},
                    }
                },
                {
                    "Счётчики", new Dictionary<string, Func<Tariff, string, bool>>
                    {
                        {"==", (t, inp) => t.Meters.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.Meters.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.Meters.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.Meters.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.Meters.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.Meters.Count <= int.Parse(inp)}
                    }
                },
                {
                    "Врем. промежутки", new Dictionary<string, Func<Tariff, string, bool>>
                    {
                        {"==", (t, inp) => t.TimeSpans.Count.Equals(int.Parse(inp))},
                        {"!=", (t, inp) => !t.TimeSpans.Count.Equals(int.Parse(inp))},
                        {">", (t, inp) => t.TimeSpans.Count > int.Parse(inp)},
                        {"<", (t, inp) => t.TimeSpans.Count < int.Parse(inp)},
                        {">=", (t, inp) => t.TimeSpans.Count >= int.Parse(inp)},
                        {"<=", (t, inp) => t.TimeSpans.Count <= int.Parse(inp)}
                    }
                },
            };

        /*Врем. промежуток*/
         static Dictionary<string, Dictionary<string, Func<TimeSpan, string, bool>>> TimeSpanDict =
            new Dictionary<string, Dictionary<string, Func<TimeSpan, string, bool>>>
            {
                {
                    "Название", new Dictionary<string, Func<TimeSpan, string, bool>>
                    {
                        {"==", (t, inp) => t.Name.Equals(inp)},
                        {"!=", (t, inp) => !t.Name.Equals(inp)},
                    }
                },
                {
                    "Время начала", new Dictionary<string, Func<TimeSpan, string, bool>>
                    {
                        {"==", (t, inp) => t.TimeStart.Equals(System.TimeSpan.Parse(inp))},
                        {"!=", (t, inp) => !t.TimeStart.Equals(System.TimeSpan.Parse(inp))},
                        {">", (t, inp) => t.TimeStart > System.TimeSpan.Parse(inp)},
                        {"<", (t, inp) => t.TimeStart < System.TimeSpan.Parse(inp)},
                        {">=", (t, inp) => t.TimeStart >= System.TimeSpan.Parse(inp)},
                        {"<=", (t, inp) => t.TimeStart <= System.TimeSpan.Parse(inp)}
                    }
                },
                {
                    "Время окончания", new Dictionary<string, Func<TimeSpan, string, bool>>
                    {
                        {"==", (t, inp) => t.TimeEnd.Equals(System.TimeSpan.Parse(inp))},
                        {"!=", (t, inp) => !t.TimeEnd.Equals(System.TimeSpan.Parse(inp))},
                        {">", (t, inp) => t.TimeEnd > System.TimeSpan.Parse(inp)},
                        {"<", (t, inp) => t.TimeEnd < System.TimeSpan.Parse(inp)},
                        {">=", (t, inp) => t.TimeEnd >= System.TimeSpan.Parse(inp)},
                        {"<=", (t, inp) => t.TimeEnd <= System.TimeSpan.Parse(inp)}
                    }
                },
            };

        /*Документ*/
         static Dictionary<string, Dictionary<string, Func<Document, string, bool>>> DocumentDict =
            new Dictionary<string, Dictionary<string, Func<Document, string, bool>>>
            {
                {
                    "Заголовок", new Dictionary<string, Func<Document, string, bool>>
                    {
                        {"==", (t, inp) => t.Title.Equals(inp)},
                        {"!=", (t, inp) => !t.Title.Equals(inp)},
                    }
                },
                {
                    "Описание", new Dictionary<string, Func<Document, string, bool>>
                    {
                        {"==", (t, inp) => t.Discription.Equals(inp)},
                        {"!=", (t, inp) => !t.Discription.Equals(inp)},
                    }
                },
                {
                    "Дата подписания", new Dictionary<string, Func<Document, string, bool>>
                    {
                        {"==", (t, inp) => t.SigningDate.Equals(DateTime.Parse(inp))},
                        {"!=", (t, inp) => !t.SigningDate.Equals(DateTime.Parse(inp))},
                        {">", (t, inp) => t.SigningDate > DateTime.Parse(inp)},
                        {"<", (t, inp) => t.SigningDate < DateTime.Parse(inp)},
                        {">=", (t, inp) => t.SigningDate >= DateTime.Parse(inp)},
                        {"<=", (t, inp) => t.SigningDate <= DateTime.Parse(inp)}
                    }
                },
            };

        static DataManager _dataManager;
        static IEnumerable<Meter> meters = _dataManager.MetRepo.Meters();
        static IEnumerable<InstalledMeter> instmeters = _dataManager.InstMetRepo.InstMaterss();
        static IEnumerable<Tariff> tariffs = _dataManager.TarRepo.Tariffs();
        static IEnumerable<User> users = _dataManager.UserRepo.Users();
        static IEnumerable<Reading> readings = _dataManager.ReadRepo.Readings();
        static IEnumerable<Document> documents = _dataManager.DocRepo.Documents();
        static IEnumerable<Type> types = _dataManager.TypeRepo.Types();
        static IEnumerable<Parametr> paparametrs = _dataManager.ParRepo.Parametrs();
        static IEnumerable<TimeSpan> timespans = _dataManager.TimeSpanRepo.TimeSpans();

        static string entity = "";

        public static IEnumerable<IConstructor> GiveCollection(DataManager dm, string req)
        {
            _dataManager = dm;
            //req =
            //"&start=(&start=(&Entity=Счётчик&String=Название&Sign=!=&Input=3&end=)&Or=И&Entity=Счётчик&String=Название&Sign=!=&Input=Вода&end=)";
            //"&start=(&start=(&Entity=Пользователь&Bool=Администратор?&Sign=!=&Input=FALSE&end=)end=)";
            req += "&";
            entity = req.Substring(req.IndexOf("&Entity=")+8).Remove(req.Substring(req.IndexOf("&Entity=") + 8).IndexOf('&'));

            switch (entity)
            {
                case "Счётчик":
                    return NewExpr(ref req);
                case "Уст.Счётчик":
                    return NewExpr(ref req);
                case "Пользователь":
                    return NewExpr(ref req);
                case "Тип":
                    return NewExpr(ref req);
                case "Показатель":
                    return NewExpr(ref req);
                case "Параметр":
                    return NewExpr(ref req);
                case "Тариф":
                    return NewExpr(ref req);
                case "Врем. промежуток":
                    return NewExpr(ref req);
                case "Документ":
                    return NewExpr(ref req);
            }

            return null;
        }

        static IEnumerable<IConstructor> NewExpr(ref string req)
        {
            string first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            string key = first.Substring(1, first.IndexOf('=') - 1);
            string value = first.Substring(first.IndexOf('=') + 1);

            req = req.Remove(0, first.Length);

            IEnumerable<IConstructor> temp;

            if (key == "start")
            {
                temp = NewExpr(ref req);
            }
            //Property
            else
            {
                //Dictionary<string, string > properties = entities[value];
                
                temp = Property(ref req);
            }

            if (req == "&") return temp;

            first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            key = first.Substring(1, first.IndexOf('=') - 1);
            value = first.Substring(first.IndexOf('=') + 1);

            if (key == "end")
            {
                req = req.Remove(0, first.Length);
                return temp;
            }

            return Logic(ref req, temp);
        }

        static IEnumerable<IConstructor> Logic(ref string req, IEnumerable<IConstructor> A)
        {
            string first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            string key = first.Substring(1, first.IndexOf('=') - 1);
            string value = first.Substring(first.IndexOf('=') + 1);

            if (key != "Or" && key != "And") return A;

            req = req.Remove(0, first.Length);

            if (key == "Or")
            {
                return NewExpr(ref req).Union(A);
            }
            else
            {
                return NewExpr(ref req).Intersect(A);
            }
        }
        
        static IEnumerable<IConstructor> Property(ref string req)
        {
            string first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            string key = first.Substring(1, first.IndexOf('=') - 1);
            string value = first.Substring(first.IndexOf('=') + 1);
            
            req = req.Remove(0, first.Length);
                
            return Sign(ref req, value);
        }

        static IEnumerable<IConstructor> Sign(ref string req, string propertyPath)
        {
            string first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            string key = first.Substring(1, first.IndexOf('=') - 1);
            string value = first.Substring(first.IndexOf('=') + 1);
            req = req.Remove(0, first.Length);

            value = value.Replace("Количество ", "");
            
            return Input(ref req, value, propertyPath);
            
        }

        static IEnumerable<IConstructor> Input(ref string req, string sign, string property)
        {
            string first = req.Remove(req.Remove(0, 1).IndexOf('&') + 1);
            string key = first.Substring(1, first.IndexOf('=') - 1);
            string value = first.Substring(first.IndexOf('=') + 1);
            req = req.Remove(0, first.Length);

            System.Type type = typeof(string);

            switch (entity)
            {
                case "Счётчик":
                    return meters.Where(t => MeterDict[property][sign](t, value));
                case "Уст. Счётчик":
                    return instmeters.Where(t => InstMeterDict[property][sign](t, value));
                case "Пользователь":
                    return users.Where(t => UserDict[property][sign](t, value));
                case "Тип":
                    return types.Where(t => TypeDict[property][sign](t, value));
                case "Показатель":
                    return readings.Where(t => ReadingDict[property][sign](t, value));
                case "Параметр":
                    return paparametrs.Where(t => ParametrDict[property][sign](t, value));
                case "Тариф":
                    return tariffs.Where(t => TariffDict[property][sign](t, value));
                case "Врем. промежуток":
                    return timespans.Where(t => TimeSpanDict[property][sign](t, value));
                case "Документ":
                    return documents.Where(t => DocumentDict[property][sign](t, value));
            }
            return null;
        }
    }
}
